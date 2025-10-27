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

        public async Task LogoutAllAsync(string refreshToken)
        {
            var tokenFromDb = await _dbContext.RefreshTokens.Where(t => t.Token == refreshToken).FirstOrDefaultAsync();
            if (tokenFromDb == null)
            {
                throw new UnauthorizedException("Refresh токен недействителен.");
            }

            await _dbContext.RefreshTokens
                .Where(t => t.UserId == tokenFromDb.UserId)
                .ExecuteDeleteAsync();

            return;
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var refreshTokenFromDb = _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken)
                ?? throw new UnauthorizedException("Refresh токен недействителен.");

            _dbContext.Remove(refreshTokenFromDb);
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<AuthServiceResponse> RefreshAsync(string refreshToken)
        {
            var refreshTokenFromDb = _dbContext.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken)
                ?? throw new UnauthorizedException("Refresh токен недействителен. Необходимо пройти аутентификацию.");

            if (refreshTokenFromDb.ExpiredAt < DateTime.UtcNow)
            {
                _dbContext.RefreshTokens.Remove(refreshTokenFromDb);
                await _dbContext.SaveChangesAsync();
                throw new UnauthorizedException("Refresh токен истек. Необходимо пройти аутентификацию.");
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
