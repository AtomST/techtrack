namespace TechTrack.NotificationService.Notification.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; } 
        public int NotificationTypeId { get; set; }
        public NotificationType? NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
