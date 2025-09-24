using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TechTrack.Shared.Logic
{
    public class JwtTokenValidator
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SecurityKey _securityKey;

        public JwtTokenValidator(string issuer, string audience, string key)
        {
            _issuer = issuer;
            _audience = audience;
            _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }

        public async Task<ClaimsIdentity?> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = _securityKey
                };

                var validationResult = await tokenHandler.ValidateTokenAsync(token, validationParameters);
                if (validationResult.IsValid)
                    return validationResult.ClaimsIdentity;
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
