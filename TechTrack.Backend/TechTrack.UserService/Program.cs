using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;
using TechTrack.UserService.Protos;

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
            // Add services to the container.
            builder.Services.AddGrpcClient<AuthService.AuthServiceClient>();
            builder.Services.AddScoped<AuthClient>();
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();
 //           app.UseMiddleware<JwtAuthenticationMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
