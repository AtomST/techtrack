using Grpc.Core;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Data;
using TechTrack.AuthService.Data.Entities;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.AuthService.Logic.Models;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;

namespace TechTrack.AuthService.Logic.Implementations
{
    public class AuthLogic : IAuthLogic
    {
        private readonly AuthServiceDbContext _dbContext;
        private readonly SecurityOptions _securityOptions;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<IAuthLogic> _logger;
        private readonly IJwtLogic _jwtLogic;
        public AuthLogic(AuthServiceDbContext dbContext, IJwtLogic jwtLogic, IOptions<SecurityOptions> options, ILogger<IAuthLogic> logger, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _jwtLogic = jwtLogic;
            _securityOptions = options.Value;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task<AuthServiceResponse> LoginAsync(LoginDto loginDto)
        {
            var credentials = _dbContext.UserCredentials.Where(u => u.Email == loginDto.Email).FirstOrDefault();
            if (credentials == null ||
                !BCrypt.Net.BCrypt.Verify(loginDto.Password, credentials.Password))
                throw new InvalidInputException("Неверный логин или пароль");
            
            var accessToken = await _jwtLogic.GenerateAccessToken(credentials.Id);
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

        public async Task<AuthServiceResponse> RegisterAsync(RegisterDto registerDto)
        {
            var credentials = _dbContext.UserCredentials.AsNoTracking().Where(u => u.Email == registerDto.Email).FirstOrDefault();
            if (credentials != null)
                throw new InvalidInputException("Пользователь с таким Email уже существует.");

            UserCredentials userCredentials = new UserCredentials()
            {
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, _securityOptions.BCryptWorkFactor)
            };
            await _dbContext.AddAsync(userCredentials);

            var accessToken = await _jwtLogic.GenerateAccessToken(userCredentials.Id, false);
            var refreshToken = _jwtLogic.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken()
            {
                Token = refreshToken,
                UserCredentials = userCredentials,
                ExpiredAt = DateTime.UtcNow.AddDays(_securityOptions.JwtRefreshTokenDurationInDays)
            };

            UserInfoCache userInfoCache = new UserInfoCache()
            {
                UserCredentials = userCredentials
            };
            await _dbContext.AddAsync(userInfoCache);
            await _dbContext.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new UserCreatedEvent
            {
                Id = userCredentials.Id,
                Email = userCredentials.Email,
                Name = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            });

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
            var newAccessToken = await _jwtLogic.GenerateAccessToken(refreshTokenFromDb.UserId);

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
