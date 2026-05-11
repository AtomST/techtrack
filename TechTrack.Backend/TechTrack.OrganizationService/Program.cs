using MassTransit;
using TechTrack.OrganizationService.Companies.gRPC;
using TechTrack.OrganizationService.Companies.Logic.Implementatios;
using TechTrack.OrganizationService.Companies.Logic.Interfaces;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Departments.Logic.gRPC;
using TechTrack.OrganizationService.Departments.Logic.Implementations;
using TechTrack.OrganizationService.Departments.Logic.Interfaces;
using TechTrack.OrganizationService.Equipments.EventHandlers;
using TechTrack.OrganizationService.Equipments.Logic.gRPC;
using TechTrack.OrganizationService.Equipments.Logic.Implementations;
using TechTrack.OrganizationService.Equipments.Logic.Interfaces;
using TechTrack.OrganizationService.UserProjections.Logic.EventHandlers;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Configuration;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService
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
            builder.Services.AddGrpc();
            builder.Services.AddAuthenticationWithoutTokenValidator();
            builder.Services.AddTechTrackAuthorization();
            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddDbContext<OrganizationServiceDbContext>();
            builder.Services.AddGrpcClient<UserIdService.UserIdServiceClient>(opt =>
            {
                opt.Address = new Uri("http://user-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<IssueRegistredEventHandler>();
                x.AddConsumer<MaintenanceAddedEventHandler>();
                x.AddConsumer<UserCreatedEventHandler>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("organization.issue-registred", e =>
                    {
                        e.AutoDelete = false;
                        e.ConfigureConsumer<IssueRegistredEventHandler>(context);
                    });

                    cfg.ReceiveEndpoint("organization.maintenance-added", e =>
                    {
                        e.AutoDelete = false;
                        e.ConfigureConsumer<MaintenanceAddedEventHandler>(context);
                    });
                    cfg.ReceiveEndpoint("organization.user-created", e =>
                    {
                        e.ConfigureConsumer<UserCreatedEventHandler>(context);
                    });
                });
            });
            builder.Services.AddScoped<ICompaniesLogic, CompaniesLogic>();
            builder.Services.AddScoped<IDepartmentsLogic, DepartmentsLogic>();
            builder.Services.AddScoped<IEquipmentsLogic, EquipmentsLogic>();

            builder.Services.AddCustomControllers();

            var app = builder.Build();
            app.MapGrpcService<ProjectionGrpcLogic>();
            app.MapGrpcService<CompanyUserGrpcLogic>();
            app.MapGrpcService<GetUserDepartmentsGrpcLogic>();

            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
