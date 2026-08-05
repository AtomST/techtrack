namespace TechTrack.NotificationService.Notification.Logic.Mappers.Models
{
    public class MaintenanceCompletedMapperModel
    {
        public string MaintenanceName { get; set; }
        public DateTime CompletedAt { get; set; }
        public string EquipmentName { get; set; }
        public string MaintenanceTypeName { get; set; }
        public string? ResponsibleUserName { get; set; }
        public string? CompletedByName { get; set; }
    }
}
