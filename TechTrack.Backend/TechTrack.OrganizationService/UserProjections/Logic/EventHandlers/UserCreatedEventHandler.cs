using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Data;
using TechTrack.OrganizationService.UserProjections.Entities;
using TechTrack.Shared.Events;

namespace TechTrack.OrganizationService.UserProjections.Logic.EventHandlers
{
    public class UserCreatedEventHandler(OrganizationServiceDbContext dbContext) : IConsumer<UserProjectionAddEvent>
    {
        public async Task Consume(ConsumeContext<UserProjectionAddEvent> context)
        {
            var message = context.Message;

            var isUserProjectionExists = await dbContext.UserProjections.AnyAsync(x => x.UserId == message.UserId);
            if (isUserProjectionExists)
                return;

            var userProjection = new UserProjection
            {
                UserId = message.UserId,
                FullName = message.FullName
            };

            await dbContext.AddAsync(userProjection);
            await dbContext.SaveChangesAsync();
        }
    }
}
