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
        private readonly IJwtLogic _jwtLogic;
        public AuthLogic(AuthServiceDbContext dbContext, IJwtLogic jwtLogic, IOptions<SecurityOptions> options)
        {
            _dbContext = dbContext;
            _jwtLogic = jwtLogic;
            _securityOptions = options.Value;
        }
        public async Task<AuthServiceResponse> Login(LoginDto loginDto)
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

        public void Logout(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
