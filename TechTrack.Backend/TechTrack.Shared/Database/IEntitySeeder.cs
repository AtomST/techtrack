using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Database
{
    public interface IEntitySeeder
    {
        int Order { get; }
        Task SeedAsync();
    }
}
