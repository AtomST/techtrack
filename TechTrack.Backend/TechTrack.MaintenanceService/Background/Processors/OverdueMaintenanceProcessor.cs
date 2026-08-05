
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Maintenances.Models;
using TechTrack.Shared.Events;

namespace TechTrack.MaintenanceService.Background.Processors
{
    public class OverdueMaintenanceProcessor(MaintenanceServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<OverdueMaintenanceProcessor> logger) : IBackgroundProcessor
    {
        public async Task ProcessAsync(CancellationToken token)
        {
            var now = DateTime.UtcNow;

            var items = await dbContext.MaintenanceLog
                .AsTracking()
                .Where(m =>
                    m.MaintenanceStatusId == (int)MaintenanceStatusTypes.Scheduled &&
                    m.ScheduledDate < now)
                .Join(
                    dbContext.EquipmentsProjection,
                    m => m.EquipmentId,
                    e => e.Id,
                    (m,e) => new {m,e}
                )
                .Join(
                    dbContext.MaintenanceSchedule,
                    me => me.m.MaintenanceScheduleRecordId!.Value,
                    s => s.Id,
                    (me,s) => new
                    {
                        Maintenance = me.m,
                        EquipmentName = me.e.Name,
                        ScheduledByUserId = s.CreatorId
                    }
                )
                .ToListAsync();

            if (items.Count == 0)
                return;

            foreach(var item in items)
            {
                item.Maintenance.MaintenanceStatusId = (int)MaintenanceStatusTypes.Overdue;

                await publishEndpoint.Publish(new MaintenanceOverdueEvent
                {
                    EquipmentName = item.EquipmentName,
                    MaintenanceId = item.Maintenance.Id,
                    MaintenanceName = item.Maintenance.Name,
                    ScheduledDate = item.Maintenance.ScheduledDate ?? DateTime.UnixEpoch,
                    ResponsibleUserId = item.Maintenance.ResponsibleUserId,
                    ScheduledByUserId = item.ScheduledByUserId
                });
            }

            await dbContext.SaveChangesAsync(token);
        }
    }
}
