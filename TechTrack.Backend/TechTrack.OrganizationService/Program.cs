using MassTransit;
using TechTrack.OrganizationService.Companies.Logic.Implementatios;
using TechTrack.OrganizationService.Companies.Logic.Interfaces;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.Departments.Logic.Implementations;
using TechTrack.OrganizationService.Departments.Logic.Interfaces;
using TechTrack.OrganizationService.Equipments.Logic.gRPC;
using TechTrack.OrganizationService.Equipments.Logic.Implementations;
using TechTrack.OrganizationService.Equipments.Logic.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;

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
            builder.Services.AddScoped<ICompaniesLogic, CompaniesLogic>();
            builder.Services.AddScoped<IDepartmentsLogic, DepartmentsLogic>();
            builder.Services.AddScoped<IEquipmentsLogic, EquipmentsLogic>();

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ModelValidationFilter>();
            });

            var app = builder.Build();
            app.MapGrpcService<ProjectionGrpcLogic>();

            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
