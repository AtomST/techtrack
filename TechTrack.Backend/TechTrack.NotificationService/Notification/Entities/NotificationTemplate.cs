namespace TechTrack.NotificationService.Notification.Entities
{
    public class NotificationTemplate
    {
        public int Id { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string TitleTemplate { get; set; }
        public string MessageTemplate { get; set; }
    }
}
