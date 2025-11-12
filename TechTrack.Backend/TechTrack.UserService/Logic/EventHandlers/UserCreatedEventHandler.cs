using MassTransit;
using TechTrack.Shared.Events;
using TechTrack.UserService.Data;
using TechTrack.UserService.Data.Entities;

namespace TechTrack.UserService.Logic.EventHandlers
{
    public class UserCreatedEventHandler : IConsumer<UserCreatedEvent>
    {
        private readonly UserServiceDbContext _dbContext;
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(UserServiceDbContext dbContext, ILogger<UserCreatedEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            User user = new User()
            {
                Id = message.Id,
                CreatedAt = message.CreatedAt,
                FullName = message.Name,
                PhoneNumber = message.PhoneNumber,
            };

            try
            {
                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                throw;
            }
        }
    }
}
