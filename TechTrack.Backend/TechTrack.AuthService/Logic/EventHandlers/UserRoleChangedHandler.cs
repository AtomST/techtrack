using MassTransit;
using TechTrack.AuthService.Data;
using TechTrack.Shared.Events;

namespace TechTrack.AuthService.Logic.EventHandlers
{
    public class UserRoleChangedHandler : IConsumer<UserRoleChanged>
    {
        private readonly AuthServiceDbContext _dbContext;

        public UserRoleChangedHandler(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<UserRoleChanged> context)
        {
            var message = context.Message;
            var userCache = _dbContext.UserInfoProjections.FirstOrDefault(u => u.UserId == message.Id);

            userCache.RoleName = message.RoleName;
            await _dbContext.SaveChangesAsync();
        }
    }
}
