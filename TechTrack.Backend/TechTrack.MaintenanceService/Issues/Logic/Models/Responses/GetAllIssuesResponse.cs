using TechTrack.MaintenanceService.Issues.Entities;

namespace TechTrack.MaintenanceService.Issues.Logic.Models.Responses
{
    public class GetAllIssuesResponse
    {
        public IList<Issue> Issues { get; set; }
    }
}
