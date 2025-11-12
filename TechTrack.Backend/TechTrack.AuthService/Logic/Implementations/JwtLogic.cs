using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TechTrack.Shared.Protos;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.AuthService.Data;
using MassTransit.Contracts;

namespace TechTrack.AuthService.Logic.Implementations
{
    public class JwtLogic : IJwtLogic
    {
        private readonly ILogger<IJwtLogic> _logger;
        //private readonly RoleService.RoleServiceClient _roleServiceClient;
        private readonly SecurityOptions _options;
        private readonly AuthServiceDbContext _dbContext;

        public JwtLogic(ILogger<JwtLogic> logger, IOptions<SecurityOptions> options, AuthServiceDbContext dbContext)
        {
            _logger = logger;
            //_roleServiceClient = roleServiceClient;
            _options = options.Value;
            _dbContext = dbContext;
        }
        public async Task<string> GenerateAccessToken(Guid userId, bool isLogin = true)
        {
            try
            {
                string userRole = string.Empty;
                string userCompanyId = string.Empty;

                if(isLogin)
                {
                    //var grpcResponse = await _roleServiceClient.GetUserRoleAsync(new GetRoleRequest { UserId = userId.ToString()});
                    //userRole = grpcResponse.UserRole;
                    var cache = await _dbContext.UserInfoCaches.FindAsync(userId);
                    if (cache is null)
                        _logger.LogWarning($"В базе есть UserCredentials, но нет UserInfoCache. UserId: {userId}");

                    userRole = cache?.RoleName ?? Roles.Undefined;
                    userCompanyId = cache?.CompanyId.ToString() ?? "";

                        
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var accessTokenKey = Encoding.ASCII.GetBytes(_options.JwtAccessTokenKey);
                var tokenDescriprot = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(ClaimTypes.Role, userRole),
                        new Claim(CustomClaimTypes.CompanyId, userCompanyId)
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
