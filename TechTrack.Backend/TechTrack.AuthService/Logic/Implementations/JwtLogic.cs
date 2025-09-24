using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Logic.Interfaces;

namespace TechTrack.AuthService.Logic.Implementations
{
    public class JwtLogic : IJwtLogic
    {
        private readonly ILogger<IJwtLogic> _logger;
        private readonly SecurityOptions _options;
        public JwtLogic(ILogger<JwtLogic> logger, IOptions<SecurityOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }
        public string GenerateAccessToken(Guid userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var accessTokenKey = Encoding.ASCII.GetBytes(_options.JwtAccessTokenKey);
                var tokenDescriprot = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim("id", userId.ToString())
                    ]),
                    Expires = DateTime.UtcNow.AddMinutes(_options.JwtAccessTokenDurationInMinutes),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(accessTokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriprot);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при генерации AccessToken: {ex.Message}");
                throw;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
