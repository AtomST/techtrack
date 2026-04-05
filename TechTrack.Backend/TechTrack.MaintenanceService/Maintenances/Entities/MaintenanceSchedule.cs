namespace TechTrack.MaintenanceService.Maintenances.Entities
{
    public class MaintenanceSchedule
    {
        public Guid Id {  get; set; }
        public Guid EquipmentId { get; set; }

        public int RecurrenceTypeId { get; set; }
        public ScheduleRecurrenceType? RecurrenceType { get; set; }

        public int IntervalValue { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public Guid ResponsibleUserId { get; set; }
        public int NotificationAdvanceDays { get; set; }
        public Guid CreatorId { get; set; }
    }
}
