using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.MaintenanceService.Issues.Logic.Models.Responses;

namespace TechTrack.MaintenanceService.Issues.Logic.Implementations
{
    public class IssuesLogic : IIssuesLogic
    {
        public Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
