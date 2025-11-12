using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;

namespace TechTrack.UserService.Logic.gRPC
{
    public class RolesGrpcLogic : RoleService.RoleServiceBase
    {
        private readonly UserServiceDbContext _dbContext;
        public RolesGrpcLogic(UserServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async override Task<GetRoleResponse> GetUserRole(GetRoleRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var guidUserId))
                throw new RpcException(new Status
                (
                    StatusCode.InvalidArgument,
                    "Неверный формат UserId."
                ));

            var user = await _dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == guidUserId)
                ?? throw new RpcException(new Status
                (
                    StatusCode.NotFound,
                    "Пользователь с таким Id не найден."
                ));

            return new GetRoleResponse
            {
                UserRole = user.Role?.Name ?? Roles.Undefined
            };
        }
    }
}
