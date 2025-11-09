using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechTrack.OrganizationService.Departments.Logic.Interfaces;
using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Responses;

namespace TechTrack.OrganizationService.Departments
{
    [ApiController]
    [Route("api/companies/{companyId}/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsLogic _departmentsLogic;

        public DepartmentsController(IDepartmentsLogic departmentsLogic)
        {
            _departmentsLogic = departmentsLogic;
        }

        [HttpPost]
        [Authorize(Policy = Policies.CompanyHeadAccess)]
        public async Task<IActionResult> CreateDepartment(Guid companyId, CreateDepartmentRequest request)
        {
            User.AdditionalPolicyValidation(companyId);

            var response = await _departmentsLogic.CreateDepartmentAsync(companyId, request);
            return Created();
        }

        [HttpGet]
        [Authorize(Policy = Policies.CompanyHeadAccess)]
        public async Task<IActionResult> GetAllDepartments(Guid companyId)
        {
            User.AdditionalPolicyValidation(companyId);

            var response = await _departmentsLogic.GetAllDepartmentsAsync(companyId);
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response.Departments
            });
        }

    }
}
