using TechTrack.OrganizationService.Data;
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

            builder.Services.AddTransient<GrpcErrorInterceptor>();
            builder.Services.AddDbContext<OrganizationServiceDbContext>();
            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
