using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.MaintenanceService.Issues
{
    [ApiController]
    [Route("api/equipments/{equipmentId:guid}/issues")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssuesLogic _issuesLogic;
        public IssuesController(IIssuesLogic issuesLogic)
        {
            _issuesLogic = issuesLogic;
        }
        [HttpPost]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> CreateIssue(Guid equipmentId, CreateIssueHttpRequest request)
        {

            var serviceRequest = new CreateIssueRequest
            {
                Name = request.Name,
                Description = request.Description,
                EquipmentId = equipmentId,
                StatusId = request.StatusId
            };
            var response = await _issuesLogic.CreateIssueAsync(serviceRequest, User.GetPrincipalInfo());
            return Created("",new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Data = new
                {
                    id = response.IssueId
                }
            });
        }
        [HttpGet]
        [Authorize(Policy =Policies.EmployeeAccess)]
        public async Task<IActionResult> GetIssues(Guid equipmentId)
        {
            var response = await _issuesLogic.GetAllIssues(equipmentId, User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode =System.Net.HttpStatusCode.OK,
                Data = response.Issues
            });
        }
    }
}
