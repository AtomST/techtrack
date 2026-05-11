using Microsoft.EntityFrameworkCore;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Logic.Interfaces;

namespace TechTrack.NotificationService.Notification.Logic.Implementations
{
    public class NotificationLogic(NotificationServiceDbContext dbContext) : INotificationLogic
    {
        public async Task<IList<Entities.Notification>> GetAllUserNotifications(Guid userId)
        {
            var notifications = await dbContext.NotificationLog
                .AsNoTracking()
                .Where(n => n.UserId == userId)
                .ToListAsync();

            return notifications;
        }
    }
}
