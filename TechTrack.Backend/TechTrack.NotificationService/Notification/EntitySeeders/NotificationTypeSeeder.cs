using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Entities;
using TechTrack.NotificationService.Notification.Models;
using TechTrack.Shared.Database;

namespace TechTrack.NotificationService.Notification.EntitySeeders
{
    public class NotificationTypeSeeder(SharedDbSeeder<NotificationServiceDbContext> dbSeeder) : IEntitySeeder
    {
        public int Order => 1;
        public async Task SeedAsync()
        {
            var data = new[]
            {
                new NotificationType {Id = (int)NotificationTypes.MaintenanceCompleted, Name = NotificationTypes.MaintenanceCompleted.ToString()},
                new NotificationType {Id = (int)NotificationTypes.MaintenanceOverdue, Name = NotificationTypes.MaintenanceOverdue.ToString()},
                new NotificationType {Id = (int)NotificationTypes.MaintenanceReminder, Name = NotificationTypes.MaintenanceReminder.ToString()},
                new NotificationType {Id = (int)NotificationTypes.CriticalIssueDetected, Name = NotificationTypes.CriticalIssueDetected.ToString()}
            };

            await dbSeeder.SeedAsync(data, x => x.Id);
        }
    }
}
