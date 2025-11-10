using Grpc.Core;
using MassTransit;
using System.Net;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Protos;
using TechTrack.UserService.Data;
using TechTrack.UserService.Logic.Interfaces;
namespace TechTrack.UserService.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {

        private readonly UserServiceDbContext _dbContext;
        private readonly CompanyUserService.CompanyUserServiceClient _companyClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserLogic(UserServiceDbContext dbContext, CompanyUserService.CompanyUserServiceClient companyClient, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _companyClient = companyClient;
            _publishEndpoint = publishEndpoint;
        }

        public async Task ChangeUserRoleAsync(Guid userId, string role, Models.UserPermissionInfo permissionInfo)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId)
                ?? throw new InvalidInputException("Пользователь с таким Id не найден");

            if(permissionInfo.Role == Roles.CompanyHead || permissionInfo.Role == Roles.Admin)
            {
                var grpcResponse = await _companyClient.IsUserInCompanyAsync(new IsUserInCompanyRequest
                {
                    UserId = userId.ToString(),
                    CompanyId = permissionInfo.CompanyId
                });

                if(!grpcResponse.IsUserInCompany)
                    throw new ForbiddenException("Вы можете менять роли только сотрудников вашей компании.");
            }

            var roleFromDb = _dbContext.Roles.FirstOrDefault(r => r.Name == role)
                ?? throw new InvalidInputException("Такая роль не найдена."); ;
            
            user.Role = roleFromDb;
            await _dbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new UserRoleChanged()
            {
                Id = userId,
                RoleName = role,
            });
        }

        public async Task ChangeUserRoleFromEvent(Guid userId, string roleName)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);

            user.RoleId = role.Id;

            await _dbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new UserRoleChanged()
            {
                Id = user.Id,
                RoleName = role.Name
            });
        }
    }
}
