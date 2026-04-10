using MassTransit;
using Microsoft.AspNetCore.Authentication;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Data;
using TechTrack.AuthService.Logic.EventHandlers;
using TechTrack.AuthService.Logic.gRPC;
using TechTrack.AuthService.Logic.Implementations;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;
namespace TechTrack.AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AuthServiceDbContext>();

            //Services
            builder.Services.AddScoped<IJwtLogic, JwtLogic>();
            builder.Services.AddScoped<IAuthLogic, AuthLogic>();

            //gRPC
            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddGrpcClient<RoleService.RoleServiceClient>(opt =>
            {
                opt.Address = new Uri("http://user-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();
            builder.Services.AddGrpc();

            //MassTransit
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserRoleChangedHandler>();
                x.AddConsumer<UserCompanyChangedHandler>();
                x.AddConsumer<CompanyRegisteredWithOwnerHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("role-changed", e =>
                    {
                        e.AutoDelete = false;
                        e.ConfigureConsumer<UserRoleChangedHandler>(context);
                    });

                    cfg.ReceiveEndpoint("company-changed", e => 
                    {
                        e.AutoDelete = false;
                        e.ConfigureConsumer<UserCompanyChangedHandler>(context);
                    });
                    cfg.ReceiveEndpoint("company-registered-withowner", e =>
                    {
                        e.AutoDelete = false;
                        e.ConfigureConsumer<CompanyRegisteredWithOwnerHandler>(context);
                    });
                });
            });

            builder.Services.AddControllers();

            builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.MapControllers();
            app.Run();
        }
    }
}
