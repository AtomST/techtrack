using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Models.Requests;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Maintenances
{
    [ApiController]
    [Route("api/equipments/{equipmentId}/maintenance")]
    public class MaintenanceController(IMaintenanceLogic maintenanceLogic) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddMaintenance(Guid equipmentId, AddMaintenanceDto requestDto)
        {
            var addMaintenanceRequest = new AddMaintenanceRequest
            (
                requestDto.Name,
                requestDto.Description,
                equipmentId,
                requestDto.SolvedIssuesId
            );

            var response = await maintenanceLogic.AddMaintenance(addMaintenanceRequest, User.GetPrincipalInfo());
            return Created();
        }
    }
}
