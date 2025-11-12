using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechTrack.AuthService.Data.Entities;
using TechTrack.Shared.Auth;

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
        public DbSet<UserInfoCache> UserInfoCaches { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("AuthServiceDbConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<UserCredentials>(entity =>
            {
                entity.ToTable("user_credentials");

                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .HasColumnName("email");
                entity.Property(e => e.Password)
                    .HasColumnName("password");

                entity
                    .HasMany(u => u.RefreshTokens)
                    .WithOne(r => r.UserCredentials)
                    .HasForeignKey(r => r.UserId);
            });


            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_tokens");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Token)
                    .HasColumnName("token");

                entity.Property(e => e.ExpiredAt)
                    .HasColumnName("expired_at")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity
                    .HasIndex(e => e.Token)
                    .IsUnique();
            });

            modelBuilder.Entity<UserInfoCache>(entity =>
            {
                entity.ToTable("user_info_cache");

                entity.HasKey(u => u.UserId);

                entity.Property(u => u.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(u => u.RoleName)
                    .HasColumnName("role_name")
                    .HasDefaultValue(Roles.Undefined);

                entity.Property(u => u.CompanyId)
                    .HasColumnName("company_id");

                entity
                    .HasOne(u => u.UserCredentials)
                    .WithOne(uc => uc.UserInfoCache)
                    .HasForeignKey<UserInfoCache>(u => u.UserId);
            });
        }
    }
}

