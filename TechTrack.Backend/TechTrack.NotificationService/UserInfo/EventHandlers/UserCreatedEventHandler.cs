using MassTransit;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Projections.Entities;
using TechTrack.Shared.Events;

namespace TechTrack.NotificationService.UserInfo.EventHandlers
{
    public class UserCreatedEventHandler(NotificationServiceDbContext dbContext) : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            var userContancts = new UserContact
            {
                UserId = message.Id,
                Name = message.Name,
                Email = message.Email
            };

            await dbContext.UserContacts
                .AddAsync(userContancts);

            await dbContext.SaveChangesAsync();
        }
    }
}
