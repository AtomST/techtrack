namespace TechTrack.MaintenanceService.Maintenances.Models.Requests
{
    public record AddMaintenanceRequest(string Name, string? Desctiprion, Guid EquipmentId, IEnumerable<Guid> SolvedIssuesId);
}
