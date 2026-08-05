namespace TechTrack.MaintenanceService.Maintenances.Models.Requests
{
    public record AddMaintenanceDto(string Name, string? Description, IEnumerable<Guid> SolvedIssuesId);
}
