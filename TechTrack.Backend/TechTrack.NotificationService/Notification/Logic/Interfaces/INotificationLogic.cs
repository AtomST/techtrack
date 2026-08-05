namespace TechTrack.NotificationService.Notification.Logic.Interfaces
{
    public interface INotificationLogic
    {
        public Task<IList<Entities.Notification>> GetAllUserNotifications(Guid userId);
    }
}
