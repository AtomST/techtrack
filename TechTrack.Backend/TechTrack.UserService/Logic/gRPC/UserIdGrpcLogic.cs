using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;

namespace TechTrack.UserService.Logic.gRPC
{
    public class UserIdGrpcLogic(UserServiceDbContext _dbContext) : UserIdService.UserIdServiceBase
    {
        public override async Task<GetUserIdByEmailResponse> GetUserIdByEmail(GetUserIdByEmailRequest request, ServerCallContext context)
        {
            var userCredentials = await _dbContext.Users
                .Where(u => u.Email == request.UserEmail)
                .FirstOrDefaultAsync()
                ?? throw new RpcException(new Status
                (
                    StatusCode.NotFound,
                    "Пользователя с таким Email не существует."
                ));

            return new GetUserIdByEmailResponse
            {
                UserId = userCredentials.Id.ToString()
            };
        }

        public override async Task<IsUserExistsResponse> IsUserExists(IsUserExistsRequest request, ServerCallContext context)
        {
            if (Guid.TryParse(request.UserId, out var guidUserId))
                throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        "Неверный формат UserId"
                    )
                );

            var isExists = await _dbContext.Users
                .Where(u => u.Id == guidUserId)
                .AnyAsync();

            return new IsUserExistsResponse
            {
                IsUserExists = isExists
            };
        }
    }
}
