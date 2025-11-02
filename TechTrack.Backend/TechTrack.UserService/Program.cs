using Grpc.Net.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using TechTrack.Shared.Auth;
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

            builder.Services.AddDbContext<UserServiceDbContext>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedEventHandler>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ReceiveEndpoint("user-created", e =>
                    {
                        e.ConfigureConsumer<UserCreatedEventHandler>(context);
                    });
                });
            });

            builder.Services.AddScoped<IUserLogic, UserLogic>();
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.MapGrpcService<RolesGrpcLogic>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
