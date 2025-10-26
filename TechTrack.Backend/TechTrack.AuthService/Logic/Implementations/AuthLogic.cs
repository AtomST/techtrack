using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Data;
using TechTrack.AuthService.Data.Entities;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.AuthService.Logic.Models;
using TechTrack.Shared.Exceptions;

namespace TechTrack.AuthService.Logic.Implementations
{
    public class AuthLogic : IAuthLogic
    {
        private readonly AuthServiceDbContext _dbContext;
        private readonly SecurityOptions _securityOptions;
        private readonly ILogger<IAuthLogic> _logger;
        private readonly IJwtLogic _jwtLogic;
        public AuthLogic(AuthServiceDbContext dbContext, IJwtLogic jwtLogic, IOptions<SecurityOptions> options, ILogger<IAuthLogic> logger)
        {
            _dbContext = dbContext;
            _jwtLogic = jwtLogic;
            _securityOptions = options.Value;
            _logger = logger;
        }
        public async Task<AuthServiceResponse> LoginAsync(LoginDto loginDto)
        {
            var credentials = _dbContext.UserCredentials.Where(u => u.Email == loginDto.Email).FirstOrDefault();
            if (credentials == null ||
                !BCrypt.Net.BCrypt.Verify(loginDto.Password, credentials.Password))
                throw new InvalidInputException("Неверный логин или пароль");
            
            var accessToken = _jwtLogic.GenerateAccessToken(credentials.Id);
            var refreshToken = _jwtLogic.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken()
            {
                ExpiredAt = DateTime.UtcNow.AddDays(_securityOptions.JwtRefreshTokenDurationInDays),
                Token = refreshToken,
                UserId = credentials.Id
            };

            credentials.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();
            return new AuthServiceResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenEntity.ExpiredAt
            };

        }

        public async Task LogoutAllAsync(Guid userId)
        {
            var tokens = await _dbContext.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            if(tokens.Count != 0)
            {
                _dbContext.RemoveRange(tokens);
                await _dbContext.SaveChangesAsync();
            }
            return;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var refreshTokenFromDb = _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
            if (refreshTokenFromDb == null || refreshTokenFromDb.ExpiredAt < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh токен уже недействителен.");
            }

            _dbContext.Remove(refreshTokenFromDb);
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<AuthServiceResponse> RefreshAsync(string refreshToken)
        {
            var refreshTokenFromDb = _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
            if (refreshTokenFromDb == null || refreshTokenFromDb.ExpiredAt < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh токен недействителен. Необходимо пройти аутентификацию.");
            }

            var newRefreshToken = _jwtLogic.GenerateRefreshToken();
            var newAccessToken = _jwtLogic.GenerateAccessToken(refreshTokenFromDb.UserId);

            refreshTokenFromDb.Token = newRefreshToken;
            refreshTokenFromDb.ExpiredAt = DateTime.UtcNow.AddDays(_securityOptions.JwtRefreshTokenDurationInDays);

            await _dbContext.SaveChangesAsync();
            return new AuthServiceResponse()
            {
                RefreshToken = newRefreshToken,
                RefreshTokenExpiredAt = refreshTokenFromDb.ExpiredAt,
                AccessToken = newAccessToken,
            };
        }
    }
}
