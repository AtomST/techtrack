using TechTrack.UserService.Models;

namespace TechTrack.UserService.Logic.Interfaces
{
    public interface IUserLogic
    {
        public Task ChangeUserRoleAsync(Guid userId, string role, UserPermissionInfo permissionInfo);

        public Task ChangeUserRoleFromEvent(Guid userId, string roleName);
    }
}
