using Microsoft.AspNetCore.Mvc;

namespace TechTrack.OrganizationService.Companies
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("CompaniesController");
        }
    }
}
