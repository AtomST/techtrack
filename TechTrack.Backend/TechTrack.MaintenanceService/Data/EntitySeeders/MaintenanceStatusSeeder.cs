using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Maintenances.Models;
using TechTrack.Shared.Database;

namespace TechTrack.MaintenanceService.Data.EntitySeeders
{
    public class MaintenanceStatusSeeder(SharedDbSeeder<MaintenanceServiceDbContext> dbSeeder) : IEntitySeeder
    {
        public async Task SeedAsync()
        {
            var data = new[]
            {
                new MaintenanceStatus { Id = 1, NameRu = "Завершено", NameEn = nameof(MaintenanceStatusTypes.Completed)},
                new MaintenanceStatus { Id = 2, NameRu = "Отменено", NameEn = nameof(MaintenanceStatusTypes.Cancelled)},
                new MaintenanceStatus { Id = 3, NameRu = "Запланировано", NameEn = nameof(MaintenanceStatusTypes.Scheduled)},
                new MaintenanceStatus { Id = 4, NameRu = "Просрочено", NameEn = nameof(MaintenanceStatusTypes.Overdue)}
            };

            await dbSeeder.SeedAsync(data, x => x.Id);
        }
    }
}
