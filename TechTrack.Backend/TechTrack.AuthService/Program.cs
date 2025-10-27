using Microsoft.AspNetCore.Authentication;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Data;
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

            builder.Services.AddScoped<IJwtLogic, JwtLogic>();
            builder.Services.AddScoped<IAuthLogic, AuthLogic>();
            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddGrpcClient<RoleService.RoleServiceClient>(opt =>
            {
                opt.Address = new Uri("http://user-service:8081");
            }).AddInterceptor<GrpcErrorInterceptor>();

            builder.Services.AddControllers();

            builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
            builder.Services.AddGrpc();

            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionHandler>();
            app.MapGrpcService<AuthGrpcLogic>();

            app.MapControllers();
            app.Run();
        }
    }
}
