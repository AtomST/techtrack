using TechTrack.NotificationService.Data;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Filters;
using TechTrack.Shared.Logic;
using TechTrack.Shared.Middleware;

namespace TechTrack.NotificationService
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
            builder.Services.AddDbContext<NotificationServiceDbContext>();
            builder.Services.AddTransient<GrpcErrorInterceptor>();
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

            app.Run();
        }
    }
}
