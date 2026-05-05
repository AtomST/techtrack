using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data.SharedEntities;
using TechTrack.MaintenanceService.Equipments.Entities;
using TechTrack.MaintenanceService.Issues.Entities;
using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Schedule.Entities;

namespace TechTrack.MaintenanceService.Data
{
    public class MaintenanceServiceDbContext : DbContext
    {
        private IConfiguration _configuration;
        public MaintenanceServiceDbContext(DbContextOptions<MaintenanceServiceDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Issue> Issues { get; set; }
        public DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
        public DbSet<EquipmentProjection> EquipmentsProjection { get; set; }

        public DbSet<Maintenance> MaintenanceLog { get; set; }
        public DbSet<MaintenanceSchedule> MaintenanceSchedule {  get; set; }
        public DbSet<MaintenanceStatus> MaintenanceStatuses { get; set; }
        public DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public DbSet<ScheduleRecurrenceType> ScheduleRecurrenceTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("MaintenanceServiceDbConnection"))
                    .UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("issues");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
                entity.Property(e => e.StatusId)
                    .HasColumnName("status_id");
                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id");
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp with time zone");
                entity.Property(e => e.EquipmentId)
                    .HasColumnName("equipment_id");
                entity.Property(e => e.IsResolved)
                    .HasColumnName("is_resolved");
                entity.Property(e => e.ResolvedByMaintenanceId)
                    .HasColumnName("resolved_by_maintenance_id");

                entity
                    .HasOne<EquipmentStatus>()
                    .WithMany(e => e.IssuesWithStatus)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne<Maintenance>()
                    .WithMany(m => m.SolvedIssues)
                    .HasForeignKey(e => e.ResolvedByMaintenanceId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
            });

            modelBuilder.Entity<EquipmentStatus>(entity =>
            {
                entity.ToTable("equipment_status");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Description)
                    .HasColumnName("description");
            });

            modelBuilder.Entity<EquipmentProjection>(entity =>
            {
                entity.ToTable("equipments_projection");

                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id");
                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id");
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.ToTable("maintenance_log");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
                entity.Property(e => e.ResponsibleUserId).HasColumnName("responsible_user_id");
                entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
                entity.Property(e => e.ScheduledDate).HasColumnName("scheduled_date");
                entity.Property(e => e.MaintenanceTypeId).HasColumnName("maintenance_type_id");
                entity.Property(e => e.MaintenanceStatusId).HasColumnName("maintenance_status_id");
            });

            modelBuilder.Entity<MaintenanceSchedule>(entity =>
            {
                entity.ToTable("maintenance_schedule");

            });

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
