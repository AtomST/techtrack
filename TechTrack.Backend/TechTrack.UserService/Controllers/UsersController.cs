using Microsoft.AspNetCore.Mvc;
using TechTrack.Shared.Responses;
using System.Net;
using TechTrack.UserService.Logic.Interfaces;
using TechTrack.UserService.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TechTrack.Shared.Auth;

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

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _userLogic.Register(dto);

            try
            {
                Cookie cookie = new Cookie
                {
                    Name = AUTH_COOKIE_NAME,
                    Value = result.RefreshToken,
                    Expires = result.RefreshTokenExpiredAt,
                    HttpOnly = true
                };
                Response.Cookies.Append(cookie.Name, cookie.Value, new CookieOptions { Expires = cookie.Expires, HttpOnly = true });


            }
            catch (Exception)
            {
                throw;
            }

            return Created("users", new SuccessResponse()
            {
                StatusCode = HttpStatusCode.Created,
                Data = new 
                {
                    accessToken = result.AccessToken
                }
            });
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
    }
}
