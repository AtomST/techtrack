using Grpc.Net.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
using TechTrack.UserService.Logic.EventHandlers;
using TechTrack.UserService.Logic.gRPC;
using TechTrack.UserService.Logic.Implementations;
using TechTrack.UserService.Logic.Interfaces;

namespace TechTrack.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton(new JwtTokenValidator
                (
                    builder.Configuration["Jwt:Issuer"],
                    builder.Configuration["Jwt:AccessTokenKey"]
                ));
            builder.Services.AddAuthenticationWithoutTokenValidator();
            builder.Services.AddTechTrackAuthorization();

            builder.Services.AddGrpc();
            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddGrpcClient<CompanyUserService.CompanyUserServiceClient>(opt =>
            {
                opt.Address = new Uri("http://organization-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddDbContext<UserServiceDbContext>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedEventHandler>();
                x.AddConsumer<CompanyRegisteredWithOwnerHandler>();
                x.AddConsumer<DepartmentCreatedWithHeadHandler>();
                x.AddConsumer<UserCompanyChangedHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });
                    cfg.ReceiveEndpoint("users.company-changed", e =>
                    {
                        e.ConfigureConsumer<UserCompanyChangedHandler>(context);
                    });
                    cfg.ReceiveEndpoint("user-created", e =>
                    {
                        e.AutoDelete = false;
                        e.Durable = true;
                        e.ConfigureConsumer<UserCreatedEventHandler>(context);
                    });

                    cfg.ReceiveEndpoint("users.company-registered-withowner", e =>
                    {
                        e.AutoDelete = false;
                        e.Durable = true;
                        e.ConfigureConsumer<CompanyRegisteredWithOwnerHandler>(context);
                    });

                    cfg.ReceiveEndpoint("users.department-created-withhead", e =>
                    {
                        e.AutoDelete = false;
                        e.Durable = true;
                        e.ConfigureConsumer<DepartmentCreatedWithHeadHandler>(context);
                    });
                });
            });

            builder.Services.AddScoped<IUserLogic, UserLogic>();
            builder.Services.AddControllers(c =>
            {
                c.Filters.Add<ModelValidationFilter>();
            });
            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            app.MapGrpcService<RolesGrpcLogic>();
            app.MapGrpcService<UserIdGrpcLogic>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
