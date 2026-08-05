using TechTrack.MaintenanceService.Schedule.Entities;

namespace TechTrack.MaintenanceService.Schedule.Models.Requests
{
    public class AddScheduleRecordRequest
    {
        public Guid EquipmentId { get; set; }
        public string MaintenanceName { get; set; }
        public string? MaintenanceDescription { get; set; }
        public int RecurrenceTypeId { get; set; }
        public int IntervalValue { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public Guid ResponsibleUserId { get; set; }
        public int NotificationAdvanceDays { get; set; }
    }
}
