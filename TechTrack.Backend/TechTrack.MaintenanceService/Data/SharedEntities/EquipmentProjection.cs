namespace TechTrack.MaintenanceService.Data.SharedEntities
{
    public class EquipmentProjection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
