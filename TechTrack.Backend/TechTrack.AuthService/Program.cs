using TechTrack.AuthService.Data;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Logic.Implementations;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.Shared.Middleware;
using TechTrack.AuthService.Logic.gRPC;
namespace TechTrack.AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AuthServiceDbContext>();
            builder.Services.AddScoped<IJwtLogic, JwtLogic>();
            builder.Services.AddControllers();
            builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
            builder.Services.AddGrpc();
            var app = builder.Build();
            app.MapGrpcService<AuthGrpcLogic>();
            app.MapControllers();
            app.Run();
        }
    }
}
