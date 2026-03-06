using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.MaintenanceService.Issues.Logic.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Issues.Logic.Interfaces
{
    public interface IIssuesLogic
    {
        public Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, UserPermissionInfo userInfo);
    }
}
