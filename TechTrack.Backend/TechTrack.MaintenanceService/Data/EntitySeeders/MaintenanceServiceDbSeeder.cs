using TechTrack.Shared.Database;

namespace TechTrack.MaintenanceService.Data.EntitySeeders
{
    public class MaintenanceServiceDbSeeder(IEnumerable<IEntitySeeder> seeders)
    {
        public async Task SeedAsync()
        {
            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
