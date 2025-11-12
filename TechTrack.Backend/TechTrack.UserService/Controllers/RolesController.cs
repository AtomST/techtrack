using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Responses;
using TechTrack.UserService.Data;

namespace TechTrack.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly UserServiceDbContext _dbContext;

        public RolesController(UserServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var result = _dbContext.Roles.ToList();
            return Ok(new SuccessResponse
            {
                Data = result,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

    }
}
