using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechTrack.AuthService.Configuration;
using TechTrack.AuthService.Data;
using TechTrack.AuthService.Data.Entities;
using TechTrack.AuthService.Logic.Interfaces;
using TechTrack.Shared.Protos;

namespace TechTrack.AuthService.Logic.gRPC
{
    public class AuthGrpcLogic : Shared.Protos.AuthService.AuthServiceBase
    {
        private readonly AuthServiceDbContext _dbContext;
        private readonly IJwtLogic _jwtLogic;
        private readonly SecurityOptions _securityOptions;
        public AuthGrpcLogic(AuthServiceDbContext dbContext, IJwtLogic jwtLogic, IOptions<SecurityOptions> options)
        {
            _dbContext = dbContext;
            _jwtLogic = jwtLogic;
            _securityOptions = options.Value;
        }
        public override Task<HelloResponse> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new HelloResponse()
                {
                    Message = $"Hello {request.Name}"
                });
        }

        public override async Task<RegisterCredentialsResponse> RegisterCredentials(RegisterCredentialsRequest request, ServerCallContext context)
        { 
            var credentials = _dbContext.UserCredentials.AsNoTracking().Where(u => u.Email == request.Email).FirstOrDefault();
            if (credentials != null)
                throw new RpcException(new Status
                (
                    StatusCode.AlreadyExists,
                    "Пользователь с таким Email уже существует."
                ));

            UserCredentials userCredentials = new UserCredentials()
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password, _securityOptions.BCryptWorkFactor)
            };
            await _dbContext.AddAsync(userCredentials);
            var accessToken = await _jwtLogic.GenerateAccessToken(userCredentials.Id);
            var refreshToken = _jwtLogic.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken()
            {
                Token = refreshToken,
                UserCredentials = userCredentials,
                ExpiredAt = DateTime.UtcNow.AddDays(_securityOptions.JwtRefreshTokenDurationInDays)
            };

            await _dbContext.AddAsync(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();
            return new RegisterCredentialsResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = refreshTokenEntity.ExpiredAt.ToString(),
                UserId = userCredentials.Id.ToString()
            };
        }
    }
}
