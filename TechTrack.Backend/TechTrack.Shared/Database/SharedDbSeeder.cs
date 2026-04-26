using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Database
{
    public class SharedDbSeeder<TContext> where TContext : DbContext
    {
        private readonly TContext _context;

        public SharedDbSeeder(TContext context)
        {
            _context = context;
        }

        public async Task SeedAsync<TEntity>(
            IEnumerable<TEntity> data,
            Func<TEntity, int> keySelector)
            where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();

            var existingKeys = await dbSet
                .AsNoTracking()
                .Select(e => keySelector(e))
                .ToListAsync();

            var existingSet = existingKeys.ToHashSet();

            var toInsert = data
                .Where(e => !existingSet.Contains(keySelector(e)))
                .ToList();

            if (toInsert.Any())
            {
                await dbSet.AddRangeAsync(toInsert);
                await _context.SaveChangesAsync();
            }
        }
    }
}
