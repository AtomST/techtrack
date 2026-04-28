namespace TechTrack.MaintenanceService.Maintenances.Models.Requests
{
    public record CompleteMaintenanceRequest()
    {
        public Guid MaintenanceId { get; init; }
        public string? Description { get; init; }
        public int? MaintenanceTypeId { get; init; }
    };
}
