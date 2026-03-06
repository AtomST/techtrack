namespace TechTrack.MaintenanceService.Issues.Logic.Models.Request
{
    public record CreateIssueRequest(string Name, string? Desctiption, Guid EquipmentId, int StatusId);
}
