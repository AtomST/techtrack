using Microsoft.EntityFrameworkCore;

namespace TechTrack.OrganizationService.Data
{
    public class OrganizationServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public OrganizationServiceDbContext(DbContextOptions<OrganizationServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("OrganizationServiceDbConnection"));
            }
        }
    }
}
