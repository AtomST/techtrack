using Grpc.Net.Client;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
using TechTrack.UserService.Logic.Implementations;
using TechTrack.UserService.Logic.Interfaces;

namespace TechTrack.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddSingleton(new JwtTokenValidator
            //    (
            //        builder.Configuration["Jwt:Issuer"],
            //        builder.Configuration["Jwt:AccessTokenKey"]
            //    ));
            //Add services to the container.
            builder.Services.AddGrpcClient<AuthService.AuthServiceClient>(opt =>
            {
                opt.Address = new Uri("http://auth-service:8081");
            });
            builder.Services.AddDbContext<UserServiceDbContext>();
            builder.Services.AddScoped<IUserLogic, UserLogic>();
            builder.Services.AddScoped<AuthClient>();
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
            //app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
