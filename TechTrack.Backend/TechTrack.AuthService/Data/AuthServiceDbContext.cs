using Microsoft.EntityFrameworkCore;
using TechTrack.AuthService.Data.Entities;

namespace TechTrack.AuthService.Data
{
    public class AuthServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        public DbSet<UserCredentials> UserCredentials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("AuthServiceDbConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCredentials>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(r => r.UserCredentials)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(r => r.Token)
                .IsUnique();
        }
    }
}

