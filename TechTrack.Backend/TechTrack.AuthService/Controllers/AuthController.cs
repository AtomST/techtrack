using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.AuthService.Logic.Models;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Responses;
using System.Security.Claims;

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
            var response = await _authLogic.LoginAsync(loginDto);

            SetRefreshToken(Response.Cookies, response.RefreshToken, response.RefreshTokenExpiredAt);

            return Ok(new SuccessResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Data = new 
                {
                    accessToken = response.AccessToken,
                }
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies.FirstOrDefault(c => c.Key == AUTH_COOKIE_NAME).Value;
            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedException("Refresh токен отсутствует. Необходима аутентификация");

             await _authLogic.LogoutAsync(refreshToken);
            SetRefreshToken(Response.Cookies, "", DateTime.UnixEpoch);

            return Ok(new SuccessResponse 
            {
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll()
        {
            var refreshToken = Request.Cookies.FirstOrDefault(c => c.Key == AUTH_COOKIE_NAME).Value;
            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedException("Refresh токен отсутствует. Необходима аутентификация");

            await _authLogic.LogoutAllAsync(refreshToken);
            SetRefreshToken(Response.Cookies, "", DateTime.UnixEpoch);

            return Ok(new SuccessResponse
            {
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies.FirstOrDefault(c => c.Key == AUTH_COOKIE_NAME).Value;
            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedException("Refresh токен отсутствует. Необходима аутентификация");

            var response = await _authLogic.RefreshAsync(refreshToken);

            SetRefreshToken(Response.Cookies, response.RefreshToken, response.RefreshTokenExpiredAt);

            return Ok(new SuccessResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Data = new
                {
                    accessToken = response.AccessToken,
                }
            });
        }

        private void SetRefreshToken(IResponseCookies cookies, string refreshToken, DateTime refreshTokenExpiredAt)
        {
            cookies.Append
            (
                AUTH_COOKIE_NAME,
                refreshToken,
                new CookieOptions()
                {
                    Expires = refreshTokenExpiredAt,
                    HttpOnly = true
                }
            );
        }
    }
}
