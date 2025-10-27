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
        public DbSet<Role> Roles { get; set; }

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

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id");

                entity
                    .HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");
            });
        }
        
    }
}
