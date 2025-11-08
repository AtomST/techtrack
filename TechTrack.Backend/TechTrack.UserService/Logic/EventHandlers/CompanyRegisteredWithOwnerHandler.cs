using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.UserService.Data;

namespace TechTrack.UserService.Logic.EventHandlers
{
    public class CompanyRegisteredWithOwnerHandler : IConsumer<CompanyRegisteredWithOwner>
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CompanyRegisteredWithOwnerHandler> _logger;

        public CompanyRegisteredWithOwnerHandler(UserServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<CompanyRegisteredWithOwnerHandler> logger)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<CompanyRegisteredWithOwner> context)
        {
            var message = context.Message;
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == message.UserId);
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == Roles.CompanyHead);

            _logger.LogInformation(role.Name);
            user.RoleId = role.Id;

            await _dbContext.SaveChangesAsync();
            await _publishEndpoint.Publish(new UserRoleChanged()
            {
                Id = user.Id,
                RoleName = Roles.CompanyHead
            });
        }
    }
}
