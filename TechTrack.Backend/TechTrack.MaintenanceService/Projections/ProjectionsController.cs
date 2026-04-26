using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Projections.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.MaintenanceService.Projections
{
    [ApiController]
    [Route("api/maintenance/[controller]")]
    public class ProjectionsController : ControllerBase
    {
        private readonly IProjectionLogic _projectionLogic;
        public ProjectionsController(IProjectionLogic projectionLogic)
        {
            _projectionLogic = projectionLogic;
        }

        [HttpPost("{departmentId:guid}")]
        [Authorize(Policy = Policies.ManagementAccess)]
        public async Task<IActionResult> ProjectionSync(Guid departmentId)
        {
            var result = await _projectionLogic.ProjectionSync(departmentId, User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = new
                {
                    SynchronizedCount = result
                },
            });
        }
    }
}
