using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TechTrack.Shared.Middleware;

namespace TechTrack.Shared.Auth
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationWithoutTokenValidator(this IServiceCollection services)
        {
            services
                .AddAuthentication("CustomScheme")
                .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("CustomScheme", opt => { });

            return services;
        }
    }
}
