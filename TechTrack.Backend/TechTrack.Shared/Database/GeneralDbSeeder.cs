using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Database
{
    public class GeneralDbSeeder(IEnumerable<IEntitySeeder> seeders)
    {
        public async Task SeedAsync()
        {
            var orderedSeeders = seeders.OrderBy(x => x.Order);

            foreach (var seeder in orderedSeeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
