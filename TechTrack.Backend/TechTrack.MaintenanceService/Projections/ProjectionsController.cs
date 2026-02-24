using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Projections
{
    [ApiController]
    [Route("api/maintenance/[controller]")]
    public class ProjectionsController : ControllerBase
    {
        [HttpPost("{departmentId:guid}")]
        [Authorize(Policy = Policies.SupervisorAccess)]
        public async Task<IActionResult> ProjectionSync(Guid departmentId)
        {
            throw new NotImplementedException();
        }
    }
}
