namespace TechTrack.MaintenanceService.Shared.Interfaces
{
    public interface ICacheLogic
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan ttl);
    }
}
