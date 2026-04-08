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
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsLogic _departmentsLogic;

        public DepartmentsController(IDepartmentsLogic departmentsLogic)
        {
            _departmentsLogic = departmentsLogic;
        }

        [HttpPost]
        [Authorize(Policy = Policies.CompanyHeadAccess)]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequest request)
        {
            var response = await _departmentsLogic.CreateDepartmentAsync(request, User.GetPrincipalInfo());
            return Created();
        }

        [HttpGet]
        [Authorize(Policy = Policies.CompanyHeadAccess)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var response = await _departmentsLogic.GetAllDepartmentsAsync(User.GetPrincipalInfo());
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response.Departments
            });
        }

        [HttpGet("{departmentId}")]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> GetFullDepartmentInfoById(Guid departmentId)
        {
            var response = await _departmentsLogic.GetFullDepartmentInfoAsync(
                departmentId,
                User.GetPrincipalInfo()
            );

            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response.Department
            });
        }
        //[HttpPost("{departmentId}/employees")]
        //[Authorize(Policy = Policies.ManagementAccess)]]
        //public async Task<IActionResult> AddEmployeeById(Guid departmentId)
        //{
            
        //}

    }
}
