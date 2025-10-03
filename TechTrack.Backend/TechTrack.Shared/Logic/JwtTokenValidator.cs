using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TechTrack.Shared.Logic
{
    public class JwtTokenValidator
    {
        private readonly string _issuer;
        private readonly SecurityKey _securityKey;

        public JwtTokenValidator(string issuer, string key)
        {
            _issuer = issuer;
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
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
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
