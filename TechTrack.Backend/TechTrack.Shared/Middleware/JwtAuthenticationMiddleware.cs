using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TechTrack.Shared.Logic;
namespace TechTrack.Shared.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, JwtTokenValidator jwtService, string accessToken)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(!string.IsNullOrEmpty(token))
            {
                var claimsIdentity = await jwtService.ValidateToken(token);
                if (claimsIdentity != null)
                {
                    context.User = new ClaimsPrincipal(claimsIdentity);
                }

            }
            await _next.Invoke(context);
        }
    }
}
