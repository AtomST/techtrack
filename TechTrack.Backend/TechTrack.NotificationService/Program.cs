using MassTransit;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.EntitySeeders;
using TechTrack.NotificationService.Notification.Logic.EventHandlers;
using TechTrack.NotificationService.Notification.Logic.Implementations;
using TechTrack.NotificationService.Notification.Logic.Interfaces;
using TechTrack.NotificationService.Notification.Logic.Mappers;
using TechTrack.NotificationService.Notification.Logic.Mappers.Models;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Database;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;

namespace TechTrack.NotificationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSingleton(new JwtTokenValidator
            (
                builder.Configuration["Jwt:Issuer"],
                builder.Configuration["Jwt:AccessTokenKey"]
            ));

            builder.Services.AddAuthenticationWithoutTokenValidator();
            builder.Services.AddTechTrackAuthorization();
            builder.Services.AddDbContext<NotificationServiceDbContext>();
            builder.Services.AddTransient<GrpcErrorInterceptor>();

            builder.Services.AddScoped(typeof(SharedDbSeeder<>));
            builder.Services.AddScoped<IEntitySeeder, NotificationTypeSeeder>();
            builder.Services.AddScoped<IEntitySeeder, NotificationTemplateSeeder>();
            builder.Services.AddScoped<IEntitySeeder, NotificationTemplateSeeder>();
            builder.Services.AddScoped<INotificationLogic, NotificationLogic>();
            builder.Services.AddScoped<TemplateDictionaryMapper>();
            builder.Services.AddScoped<ITemplateModelMapper<MaintenanceCompletedMapperModel>, MaintenanceCompletedMapper>();
            builder.Services.AddScoped<ITemplateModelMapper<MaintenanceOverdueMapperModel>, MaintenanceOverdueMapper>();
            builder.Services.AddScoped<ITemplateModelMapper<CriticalIssueDetectedMapperModel>, CriticalIssueDetectedMapper>();
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<MaintenanceCompletedHandler>();
                x.AddConsumer<MaintenanceOverdueHandler>();
                x.AddConsumer<CriticalIssueDetectedHandler>();
                x.AddEntityFrameworkOutbox<NotificationServiceDbContext>(c =>
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

                    cfg.ReceiveEndpoint("notification.maintenance-completed", e =>
                    {
                        e.ConfigureConsumer<MaintenanceCompletedHandler>(context);
                    });
                    cfg.ReceiveEndpoint("notification.maintenance-overdue", e =>
                    {
                        e.ConfigureConsumer<MaintenanceOverdueHandler>(context);
                    });
                    cfg.ReceiveEndpoint("notificaiton.criticalissue-detected", e =>
                    {
                        e.ConfigureConsumer<CriticalIssueDetectedHandler>(context);
                    });
                });
            });
            builder.Services.AddScoped<GeneralDbSeeder>();
            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidationFilter>();
            });

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<GeneralDbSeeder>();
                await seeder.SeedAsync();
            }
            app.Run();
        }
    }
}
