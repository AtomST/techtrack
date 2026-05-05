namespace TechTrack.MaintenanceService.Issues.Logic.Models.Request
{
    public record CreateIssueRequest(string Name, string? Description, Guid EquipmentId, int StatusId);
}
