using TechTrack.MaintenanceService.Issues.Entities;

namespace TechTrack.MaintenanceService.Maintenances.Entities
{
    public class Maintenance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public Guid EquipmentId {  get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<Issue> SolvedIssues { get; set; } = new List<Issue>();
    }
}
