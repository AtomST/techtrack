using MassTransit;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Events;

namespace TechTrack.OrganizationService.Equipments.EventHandlers
{
    public class MaintenanceAddedEventHandler(OrganizationServiceDbContext dbContext, ILogger<MaintenanceAddedEventHandler> logger) : IConsumer<MaintenanceAdded>
    {
        public async Task Consume(ConsumeContext<MaintenanceAdded> context)
        {
            var message = context.Message;
            var equipment = dbContext.Equipments
                .FirstOrDefault(e => e.Id == message.EquipmentId);

            if (equipment == null)
            {
                logger.LogWarning($"Добавлено обслуживание к несуществующему оборудованию (id: {message.EquipmentId}");
                return;
            }

            if(equipment.CurrentStatusId > message.MaxIssueStatusCode)
            {
                equipment.CurrentStatusId = message.MaxIssueStatusCode;
                await dbContext.SaveChangesAsync();
            }
            return;
        }
    }
}
