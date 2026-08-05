using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.UserService.Data;

namespace TechTrack.UserService.Logic.EventHandlers
{
    public class UserCompanyChangedHandler(UserServiceDbContext _dbContext, IPublishEndpoint _publishEndpoint) : IConsumer<UserCompanyChanged>
    {
        public async Task Consume(ConsumeContext<UserCompanyChanged> context)
        {
            var message = context.Message;
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == message.UserId);

            var newRole = ResolveNewRole(user?.Role?.Name, message.CompanyId != null);

            if (newRole == null || newRole == user?.Role?.Name)
                return;

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == newRole);
            user.Role = role;

            await _dbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new UserRoleChanged()
            {
                Id = message.UserId,
                RoleName = role.Name
            });
        }

        private string? ResolveNewRole(string? userRole, bool hasCompany)
        {
            return (userRole, hasCompany) switch
            {
                (null, true) => Roles.Employee,
                (Roles.Undefined, true) => Roles.Employee,
                (not null, false) => Roles.Undefined,
                _ => null
            };
        }
    }
}
