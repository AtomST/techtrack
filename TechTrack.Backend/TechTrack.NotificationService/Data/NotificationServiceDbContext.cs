using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.NotificationService.Notification.Entities;
using TechTrack.NotificationService.Projections.Entities;

namespace TechTrack.NotificationService.Data
{
    public class NotificationServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("NotificationServiceDbConnection"))
                    .UseSnakeCaseNamingConvention();
            }
        }
        public DbSet<Notification.Entities.Notification> NotificationLog {  get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
