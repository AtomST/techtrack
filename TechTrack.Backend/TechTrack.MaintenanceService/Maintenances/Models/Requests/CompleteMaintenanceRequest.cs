namespace TechTrack.MaintenanceService.Maintenances.Models.Requests
{
    public record CompleteMaintenanceRequest(Guid MaintenanceId, string? Description, int? MaintenanceTypeId);
}
