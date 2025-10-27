using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
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

            builder.Services
                .AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("CustomScheme", opt => { });
            builder.Services.AddGrpc();
            builder.Services.AddTransient<GrpcErrorInterceptor>();

            builder.Services.AddGrpcClient<AuthService.AuthServiceClient>(opt =>
            {
                opt.Address = new Uri("http://auth-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();

            builder.Services.AddDbContext<UserServiceDbContext>();

            builder.Services.AddScoped<IUserLogic, UserLogic>();
            builder.Services.AddScoped<AuthClient>();
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
