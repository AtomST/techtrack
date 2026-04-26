using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Schedule.Interfaces;
using TechTrack.MaintenanceService.Schedule.Models.Requests;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.MaintenanceService.Schedule
{
    [ApiController]
    [Route("api/equipments/{equipmentId:guid}/schedule")]
    public class ScheduleController(IScheduleLogic scheduleLogic) : ControllerBase
    {
        [HttpPost]
        [Authorize(Policy = Policies.ManagementAccess)]
        public async Task<IActionResult> AddScheduleRecord(Guid equipmentId, [FromBody]AddScheduleRecordHttpRequest httpRequest)
        {
            var userInfo = User.GetPrincipalInfo();
            var addScheduleRequest = new AddScheduleRecordRequest
            {
                EquipmentId = equipmentId,
                MaintenanceName = httpRequest.MaintenanceName,
                IntervalValue = httpRequest.IntervalValue,
                NextMaintenanceDate = httpRequest.NextMaintenanceDate,
                NotificationAdvanceDays = httpRequest.NotificationAdvanceDays,
                RecurrenceTypeId = httpRequest.RecurrenceTypeId,
                ResponsibleUserId = httpRequest.ResponsibleUserId
            };

            var response = await scheduleLogic.AddScheduleRecord(addScheduleRequest, userInfo);
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Data = response
            });
        }
    }
}
