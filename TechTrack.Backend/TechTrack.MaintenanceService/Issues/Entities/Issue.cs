using TechTrack.OrganizationService.Equipments.Entities;

namespace TechTrack.MaintenanceService.Issues.Entities
{
    public class Issue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public Guid EquipmentId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatorId { get; set; }

        public EquipmentStatus? EquipmentStatus { get; set; }
    }
}
