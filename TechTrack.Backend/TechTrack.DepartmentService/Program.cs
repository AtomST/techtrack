using TechTrack.Shared.Logic;

namespace TechTrack.DepartmentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSingleton(new JwtTokenValidator
                (
                    builder.Configuration["Jwt:Issuer"],
                    builder.Configuration["Jwt:AccessTokenKey"]
                ));
            var app = builder.Build();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
