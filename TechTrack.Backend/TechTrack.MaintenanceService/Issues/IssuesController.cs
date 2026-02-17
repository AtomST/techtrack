using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Issues
{
    [ApiController]
    [Route("api/maintenance/[controller]")]
    public class IssuesController : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public Task<IActionResult> CreateIssue()
        {

            throw new NotImplementedException();
        }
    }
}
