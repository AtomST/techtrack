
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Maintenances.Models;

namespace TechTrack.MaintenanceService.Background.Processors
{
    public class OverdueMaintenanceProcessor(MaintenanceServiceDbContext dbContext, IPublishEndpoint publishEndpoint) : IBackgroundProcessor
    {
        public async Task ProcessAsync(CancellationToken token)
        {
            var now = DateTime.UtcNow;

            var items = await dbContext.MaintenanceLog
                .Where(m =>
                    m.MaintenanceStatusId == (int)MaintenanceStatusTypes.Scheduled &&
                    m.ScheduledDate < now)
                .ToListAsync();

            if (items.Count == 0)
                return;

            foreach(var item in items)
            {
                item.MaintenanceStatusId = (int)MaintenanceStatusTypes.Overdue;
                
                //Публикация события в будущем.
            }

            await dbContext.SaveChangesAsync(token);
        }
    }
}
