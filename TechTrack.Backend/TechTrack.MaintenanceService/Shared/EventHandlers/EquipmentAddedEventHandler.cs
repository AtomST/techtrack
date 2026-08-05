using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Data.SharedEntities;
using TechTrack.Shared.Events;

namespace TechTrack.MaintenanceService.Shared.EventHandlers
{
    public class EquipmentAddedEventHandler(MaintenanceServiceDbContext dbContext) : IConsumer<EquipmentAddedEvent>
    {
        public async Task Consume(ConsumeContext<EquipmentAddedEvent> context)
        {
            var message = context.Message;
            var isExists = await dbContext.EquipmentsProjection
                .AsNoTracking()
                .Where(x => x.Id == message.EquipmentId)
                .AnyAsync();

            if (isExists)
                return;

            var equipmentProjection = new EquipmentProjection
            {
                Id = message.EquipmentId,
                DepartmentId = message.DepartmentId,
                CompanyId = message.CompanyId,
                Name = message.Name,
                ResponsibleUserId = message.ResponsibleUserId,
            };

            await dbContext.AddAsync(equipmentProjection);
            await dbContext.SaveChangesAsync();
        }
    }
}
