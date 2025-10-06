using Grpc.Core;
using System.Net;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
using TechTrack.UserService.Data.Entities;
using TechTrack.UserService.Logic.Interfaces;
using TechTrack.UserService.Models.Requests;
using TechTrack.UserService.Models.Responses;

namespace TechTrack.UserService.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {
        private readonly AuthService.AuthServiceClient _authClient;
        private readonly UserServiceDbContext _dbContext;
        public UserLogic(AuthService.AuthServiceClient authClient, UserServiceDbContext dbContext)
        {
            _authClient = authClient;
            _dbContext = dbContext;
        }
        public async Task<UserLogicResponse> Register(RegisterDto dto)
        {

            var registerUserCredentialsResult = await _authClient.RegisterCredentialsAsync(
            new RegisterCredentialsRequest()
            {
                Email = dto.Email,
                Password = dto.Password
            });

            User user = new User()
            {
                Id = Guid.Parse(registerUserCredentialsResult.UserId),
                CreatedAt = DateTime.UtcNow,
                FullName = dto.FullName,
            };

            try
            {
                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return new UserLogicResponse
            (
                registerUserCredentialsResult.AccessToken,
                registerUserCredentialsResult.RefreshToken,
                DateTime.Parse(registerUserCredentialsResult.RefreshTokenExpiredAt),
                registerUserCredentialsResult.UserId
            );
        }
    }
}
