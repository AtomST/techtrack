namespace TechTrack.MaintenanceService.Issues.Logic.Models.Request
{
    public record CreateIssueHttpRequest(string Name, string? Description, int StatusId);
}
