namespace TechTrack.MaintenanceService.Maintenances.Models.Requests
{
    public record CompleteMaintenanceHttpRequest(string? Description, int? MaintenanceTypeId);
}
