using MassTransit;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Issues.Logic.Implementations;
using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Implementations;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Projections.Implementations;
using TechTrack.MaintenanceService.Projections.Interfaces;
using TechTrack.MaintenanceService.Schedule.Implementations;
using TechTrack.MaintenanceService.Schedule.Interfaces;
using TechTrack.MaintenanceService.Shared.Implementations;
using TechTrack.MaintenanceService.Shared.Interfaces;
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

            builder.Services.AddGrpcClient<UserDepartmentsService.UserDepartmentsServiceClient>(opt =>
            {
                opt.Address = new Uri("http://organization-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidationFilter>();
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<MaintenanceServiceDbContext>(c =>
                {
                    c.UsePostgres();
                    c.UseBusOutbox();
                });
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
            builder.Services.AddScoped<IMaintenanceLogic, MaintenanceLogic>();
            builder.Services.AddScoped<ICacheLogic, CacheLogic>();
            builder.Services.AddScoped<IScheduleLogic, ScheduleLogic>();
            builder.Services.AddScoped<IUserDepartmentLogic, UserDepartmentLogic>();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
