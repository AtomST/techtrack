using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTrack.OrganizationService.Companies.Logic.Interfaces;
using TechTrack.OrganizationService.Companies.Models.Requests;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Responses;

namespace TechTrack.OrganizationService.Companies
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompaniesLogic _companiesLogic;

        public CompaniesController(ICompaniesLogic companiesLogic)
        {
            _companiesLogic = companiesLogic;
        }

        [HttpPost]
        [Authorize(Policy = Policies.PlatformAdminAccess)]
        public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyRequest request)
        {
            //FluentValid in future
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Address))
                throw new InvalidInputException("Все поля должы быть заполнены");

            var result = await _companiesLogic.RegisterAsync(request);

            return Ok(new SuccessResponse()
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Data = new
                {
                    result.CompanyId
                }
            });
        }



        //[HttpGet("{companyId:guid}")]
        //[Authorize(Roles = Policies.EmployeeAccess)]
        //public async Task<IActionResult> GetCompanyById()
        //{
        //    var result = await _companiesLogic.RegisterAsync(request);

        //    return CreatedAtAction
        //}
    }
}
