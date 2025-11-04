using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Responses;
using System.Net;
using TechTrack.UserService.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TechTrack.Shared.Auth;
using TechTrack.UserService.Models;
using TechTrack.UserService.Models.Requests;

namespace TechTrack.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserLogic _userLogic;
        private readonly string AUTH_COOKIE_NAME = "techtrack_refresh_token";
        public UsersController(IUserLogic userLogic, IConfiguration configuration)
        {
            _userLogic = userLogic;
            AUTH_COOKIE_NAME = configuration["Security:AuthHttpOnlyCookieName"];
        }
        [HttpGet("exception")]
        public async Task<IActionResult> Exc()
        {
            throw new NotImplementedException();
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> Test(string name)
        {
            return Ok();
        }
        [HttpGet("secured")]
        [Authorize]
        public async Task<IActionResult> Secured()
        {
            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        [HttpGet("dev")]
        [Authorize(Policy = Policies.DevOnly)]
        public async Task<IActionResult> TestDevOnly()
        {
            return Ok(new
            {
                Id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Role = User.FindFirstValue(ClaimTypes.Role)
            });
        }

        [HttpPatch("{userId:guid}/role")]
        [Authorize(Policy = Policies.AdminAccess)]
        public async Task<IActionResult> ChangeUserRole(Guid userId, [FromBody] ChangeRoleRequest request)
        {
            var permissionInfo = new UserPermissionInfo()
            {
                Role = User.FindFirstValue(ClaimTypes.Role),
                CompanyId = User.FindFirstValue(CustomClaimTypes.CompanyId)
            };

            await _userLogic.ChangeUserRoleAsync(userId, request.Role, permissionInfo);
            return Ok(new SuccessResponse 
            {
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
