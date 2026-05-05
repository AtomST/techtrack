namespace TechTrack.NotificationService.Notification.Logic.Mappers.Models
{
    public class MaintenanceOverdueMapperModel
    {
        public string MaintenanceName { get; set; }
        public string EquipmentName { get; set; }
        public string? ResponsibleUserName { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
