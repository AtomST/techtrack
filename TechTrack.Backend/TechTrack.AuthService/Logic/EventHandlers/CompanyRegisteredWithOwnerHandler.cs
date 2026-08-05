using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.AuthService.Data;
using TechTrack.Shared.Events;

namespace TechTrack.AuthService.Logic.EventHandlers
{
    public class CompanyRegisteredWithOwnerHandler : IConsumer<CompanyRegisteredWithOwner>
    {
        private readonly AuthServiceDbContext _dbContext;

        public CompanyRegisteredWithOwnerHandler(AuthServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CompanyRegisteredWithOwner> context)
        {
            var message = context.Message;
            var userCache = _dbContext.UserInfoProjections.FirstOrDefault(u => u.UserId == message.UserId);

            userCache.CompanyId = message.CompanyId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
