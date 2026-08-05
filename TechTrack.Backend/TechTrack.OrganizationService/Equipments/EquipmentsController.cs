using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TechTrack.OrganizationService.Equipments.Logic.Interfaces;
using TechTrack.OrganizationService.Equipments.Models.Requests;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.OrganizationService.Equipments
{
    [ApiController]
    [Route("api/departments/{departmentId}/[controller]")]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentsLogic _equipmentsLogic;

        public EquipmentsController(IEquipmentsLogic equipmentsLogic)
        {
            _equipmentsLogic = equipmentsLogic;
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminAccess)]
        public async Task<IActionResult> AddEquipment(Guid departmentId, AddEquipmentRequest request)
        {
            var response = await _equipmentsLogic.AddEquipmentAsync(departmentId, request, User.GetPrincipalInfo());

            return CreatedAtRoute(
                "",
                response.EquipmentId,
                new SuccessResponse
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = response
                }
            );
        }

        [HttpGet]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> GetEquipments(Guid departmentId)
        {
            var response = await _equipmentsLogic.GetAllEquipmentsAsync(departmentId, User.GetPrincipalInfo());

            return Ok(new SuccessResponse
            {
                StatusCode = HttpStatusCode.OK,
                Data = response.Equipments
            });
        }
    }
}
