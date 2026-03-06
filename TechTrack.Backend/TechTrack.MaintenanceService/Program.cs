using MassTransit;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Issues.Logic.Implementations;
using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Projections.Implementations;
using TechTrack.MaintenanceService.Projections.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;

namespace TechTrack.MaintenanceService
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

            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddGrpcClient<ProjectionService.ProjectionServiceClient>(opt =>
            {
                opt.Address = new Uri("http://organization-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidationFilter>();
            });

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });
                });
            });
            builder.Services.AddControllers();
            builder.Services.AddDbContext<MaintenanceServiceDbContext>();
            builder.Services.AddScoped<IProjectionLogic, ProjectionLogic>();
            builder.Services.AddScoped<IIssuesLogic, IssuesLogic>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
