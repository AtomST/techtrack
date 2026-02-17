using TechTrack.MaintenanceService.Issues.Entities;

namespace TechTrack.OrganizationService.Equipments.Entities
{
    public class EquipmentStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public IList<Issue> IssuesWithStatus { get; set; } = new List<Issue>();
    }
}
