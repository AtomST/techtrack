using TechTrack.Shared.Middleware;
using Yarp.ReverseProxy;

namespace TechTrack.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true) // Разрешить любой origin в Development
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();
            app.UseCors("AllowFrontend");
            app.UseMiddleware<GlobalExceptionHandler>();

            app.MapReverseProxy();
            app.UseAuthorization();
            app.Run();
        }
    }
}
