using TechTrack.MaintenanceService.Shared.Interfaces;

namespace TechTrack.MaintenanceService.Shared.Implementations
{
    public class UserDepartmentCacheHelper : IUserDepartmentCacheHelper
    {
        public string GetUserDepartmentCacheKey(Guid userId)
            => $"user:{userId}:departments";

        public TimeSpan GetUserDepartmentCacheTTL()
            => TimeSpan.FromDays(7);
    }
}
