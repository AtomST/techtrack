using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TechTrack.Shared.Middleware
{
    public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public JwtAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder
            )
            : base(options, logger, encoder)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                var ticket = new AuthenticationTicket(Context.User, "CustomScheme");
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
