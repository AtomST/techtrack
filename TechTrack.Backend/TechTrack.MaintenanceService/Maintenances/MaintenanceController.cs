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
        [HttpPost]
        [Route("~/api/maintenances/{maintenanceId:guid}/complete")]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> CompleteMaintenance(Guid maintenanceId, CompleteMaintenanceHttpRequest request)
        {
            var completeMaintenanceRequest = new CompleteMaintenanceRequest
            {
                MaintenanceId = maintenanceId,
                Description = request.Description,
                MaintenanceTypeId = request.MaintenanceTypeId
            };

            await maintenanceLogic.CompleteMaintenanceAsync(completeMaintenanceRequest, User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        [HttpGet]
        [Route("~/api/departments/{departmentId}/maintenances")]
        [Authorize(Policy = Policies.ManagementAccess)]
        public async Task<IActionResult> GetAllDepartmentMaintenances(Guid departmentId)
        {

            var response = await maintenanceLogic.GetAllDepartmentMaintenances(departmentId, User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response.MaintenanceLog
            });
        }
    }
}
