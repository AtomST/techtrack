using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Companies.Entities;
using TechTrack.OrganizationService.Departments.Entities;
using TechTrack.OrganizationService.Equipments.Entities;

namespace TechTrack.OrganizationService.Data
{
    public class OrganizationServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public OrganizationServiceDbContext(DbContextOptions<OrganizationServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentUser> UserDepartments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("OrganizationServiceDbConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("companies");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Address)
                    .HasColumnName("address");

                entity.Property(e => e.ConnectedAt)
                    .HasColumnName("connected_at")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            });

            modelBuilder.Entity<CompanyUser>(entity =>
            {
                entity.ToTable("company_user");
                entity.HasKey(e => new { e.UserId, e.CompanyId });

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id");

                entity.Property(e => e.JoinedAt)
                    .HasColumnName("joined_at")
                    .HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id");

                entity.Property(e => e.ResponsibleUserId)
                    .HasColumnName("responsible_user_id");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipments");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
                entity.Property(e => e.SerialNumber)
                    .HasColumnName("serialNumber");
                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id");
                entity.Property(e => e.CurrentStatusId)
                    .HasColumnName("current_status_id");
            });

            modelBuilder.Entity<DepartmentUser>(entity =>
            {
                entity.ToTable("department_user");

                entity.HasKey(e => e.UserId);

                entity.Property(p => p.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id");
                entity.Property(e => e.JoinedAt)
                    .HasColumnName("joined_at");

            });

        }
    }
}
