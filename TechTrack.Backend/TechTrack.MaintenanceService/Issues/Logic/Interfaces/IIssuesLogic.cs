using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.MaintenanceService.Issues.Logic.Models.Responses;

namespace TechTrack.MaintenanceService.Issues.Logic.Interfaces
{
    public interface IIssuesLogic
    {
        public Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request);
    }
}
