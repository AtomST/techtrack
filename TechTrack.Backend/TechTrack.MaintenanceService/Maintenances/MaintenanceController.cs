using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Models.Requests;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.MaintenanceService.Maintenances
{
    [ApiController]
    [Route("api/equipments/{equipmentId}/maintenance")]
    public class MaintenanceController(IMaintenanceLogic maintenanceLogic) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = Policies.EmployeeAccess)]
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
            return Created("", new SuccessResponse { StatusCode = System.Net.HttpStatusCode.Created});
        }

        [HttpGet]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> GetMaintenanceLog(Guid equipmentId)
        {
            var response = await maintenanceLogic.GetMaintenanceLogByEquipment(equipmentId, User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response.MaintenanceLog
            });
        }
    }
}
