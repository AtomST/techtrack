namespace TechTrack.MaintenanceService.Issues.Logic.Models.Request
{
    public record CreateIssueRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid EquipmentId { get; set; }
        public int StatusId { get; set; }
    }
}
