namespace TechTrack.MaintenanceService.Shared.Interfaces
{
    public interface IUserDepartmentCacheHelper
    {
        public string GetUserDepartmentCacheKey(Guid userId);
        public TimeSpan GetUserDepartmentCacheTTL();
    }
}
