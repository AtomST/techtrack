namespace TechTrack.OrganizationService.Equipments.Entities
{
    public class Equipment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string? Description { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public int CurrentStatusId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
