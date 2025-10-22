using Microsoft.AspNetCore.Mvc;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.AuthService.Logic.Models;
using TechTrack.Shared.Responses;

namespace TechTrack.AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthLogic _authLogic;
        private readonly string AUTH_COOKIE_NAME = "techtrack_refresh_token";
        public AuthController(IAuthLogic authLogic, IConfiguration configuration)
        {
            _authLogic = authLogic;
            AUTH_COOKIE_NAME = configuration["Security:AuthHttpOnlyCookieName"];
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authLogic.Login(loginDto);
            Response.Cookies.Append
            (
                AUTH_COOKIE_NAME, 
                response.RefreshToken, 
                new CookieOptions() 
                    { 
                        Expires = response.RefreshTokenExpiredAt, 
                        HttpOnly = true 
                }
            );

            return Ok(new SuccessResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = new 
                {
                    accessToken = response.AccessToken,
                }
            });
        }
    }
}
