using Microsoft.EntityFrameworkCore;
using TechTrack.UserService.Data.Entities;

namespace TechTrack.UserService.Data
{
    public class UserServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("UserServiceDbConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
        
    }
}
