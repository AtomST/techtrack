using TechTrack.MaintenanceService.Schedule.Entities;
using TechTrack.MaintenanceService.Schedule.Models;
using TechTrack.Shared.Database;

namespace TechTrack.MaintenanceService.Data.EntitySeeders
{
    public class ScheduleRecurrenceTypeSeeder(SharedDbSeeder<MaintenanceServiceDbContext> dbSeeder) : IEntitySeeder
    {
        public async Task SeedAsync()
        {
            var data = new[]
            {
                new ScheduleRecurrenceType { Id = 1, NameRu = "День", NameEn = nameof(ScheduleRecurrenceTypes.Day), InDaysValue = 1 },
                new ScheduleRecurrenceType { Id = 2, NameRu = "Неделя", NameEn = nameof(ScheduleRecurrenceTypes.Week), InDaysValue = 7 },
                new ScheduleRecurrenceType { Id = 3, NameRu = "Месяц", NameEn = nameof(ScheduleRecurrenceTypes.Month), InDaysValue = 31 }
            };

            await dbSeeder.SeedAsync(data, x => x.Id);
        }
    }
}
