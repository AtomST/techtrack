using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.AuthService.Data;
using TechTrack.Shared.Protos;

namespace TechTrack.AuthService.Logic.gRPC
{
    public class UserIdGrpcLogic(AuthServiceDbContext _dbContext) : UserIdService.UserIdServiceBase
    {
        public override async Task<GetUserIdByEmailResponse> GetUserIdByEmail(GetUserIdByEmailRequest request, ServerCallContext context)
        {
            var userCredentials = await _dbContext.UserCredentials
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
    }
}
