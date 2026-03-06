using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.MaintenanceService.Issues
{
    [ApiController]
    [Route("api/maintenance/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssuesLogic _issuesLogic;
        public IssuesController(IIssuesLogic issuesLogic)
        {
            _issuesLogic = issuesLogic;
        }
        [HttpPost]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> CreateIssue([FromBody] CreateIssueRequest request)
        {

            var userInfo = await _issuesLogic.CreateIssueAsync(request, User.GetPrincipalInfo());
            return Created("",new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Data = new
                {
                    id = userInfo.IssueId
                }
            });
        }
    }
}
