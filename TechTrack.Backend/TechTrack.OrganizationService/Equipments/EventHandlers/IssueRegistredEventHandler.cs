using MassTransit;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Events;

namespace TechTrack.OrganizationService.Equipments.EventHandlers
{
    public class IssueRegistredEventHandler : IConsumer<IssueRegistred>
    {
        private readonly OrganizationServiceDbContext _dbContext;
        private readonly ILogger<IssueRegistredEventHandler> _logger;
        public IssueRegistredEventHandler(OrganizationServiceDbContext dbContext, ILogger<IssueRegistredEventHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IssueRegistred> context)
        {
            var message = context.Message;

            var equipment = _dbContext.Equipments
                .FirstOrDefault(e => e.Id == message.EquipmentId);
            if(equipment == null)
            {
                _logger.LogWarning("В IssueRegistredEventHandler поступил несуществующий equipment_id ");
                return;
            }

            if(message.StatusId > equipment.CurrentStatusId)
                equipment.CurrentStatusId = message.StatusId;

            await _dbContext.SaveChangesAsync();
        }
    }
}
