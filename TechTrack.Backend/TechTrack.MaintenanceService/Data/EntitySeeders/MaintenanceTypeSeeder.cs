using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Maintenances.Models;
using TechTrack.Shared.Database;

namespace TechTrack.MaintenanceService.Data.EntitySeeders
{
    public class MaintenanceTypeSeeder(SharedDbSeeder<MaintenanceServiceDbContext> dbSeeder) : IEntitySeeder
    {
        public int Order => 1;

        public async Task SeedAsync()
        {
            var data = new[]
            {
                new MaintenanceType { Id = 1, NameRu = "Профилактика", NameEn = nameof(MaintenanceTypes.Preventive)},
                new MaintenanceType { Id = 2, NameRu = "Ремонт", NameEn = nameof(MaintenanceTypes.Repair)},
                new MaintenanceType { Id = 3, NameRu = "Модернизация", NameEn = nameof(MaintenanceTypes.Upgrade)},
            };

            await dbSeeder.SeedAsync(data, x => x.Id);
        }
    }
}
