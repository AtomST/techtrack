using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TechTrack.MaintenanceService.Shared.Interfaces;

namespace TechTrack.MaintenanceService.Shared.Implementations
{
    public class CacheLogic(IDistributedCache _cache) : ICacheLogic
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _cache.GetStringAsync(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = ttl,
            };

            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, option);
        }
    }
}
