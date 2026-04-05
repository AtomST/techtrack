using TechTrack.MaintenanceService.Issues.Entities;

namespace TechTrack.MaintenanceService.Maintenances.Entities
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public Guid EquipmentId {  get; set; }
        public Guid? ScheduleRecordId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? ComplitedAt { get; set; }
        public Guid ResponsibleUserId { get; set; }
        public int MaintenanceTypeId { get; set; }
        public MaintenanceType? Type { get; set; }
        public int MaintenanceStatusId { get; set; }
        public MaintenanceStatus? Status { get; set; }
        public IEnumerable<Issue> SolvedIssues { get; set; } = new List<Issue>();
    }
}
