using TechTrack.MaintenanceService.Maintenances.Entities;

namespace TechTrack.MaintenanceService.Schedule.Entities
{
    public class MaintenanceSchedule
    {
        public Guid Id {  get; set; }
        public Guid EquipmentId { get; set; }
        public string MaintenanceName { get; set; }
        public string? MaintenanceDescription { get; set; }
        public int RecurrenceTypeId { get; set; }
        public ScheduleRecurrenceType? RecurrenceType { get; set; }
        public int IntervalValue { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public Guid ResponsibleUserId { get; set; }
        public int NotificationAdvanceDays { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<Maintenance> ScheduledMaintenances { get; set; } = new List<Maintenance>();
    }
}
