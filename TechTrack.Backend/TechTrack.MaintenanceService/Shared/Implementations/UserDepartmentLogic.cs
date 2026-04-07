using TechTrack.MaintenanceService.Shared.Interfaces;
using TechTrack.Shared.Protos;

namespace TechTrack.MaintenanceService.Shared.Implementations
{
    public class UserDepartmentLogic(
        ICacheLogic _cache, 
        UserDepartmentsService.UserDepartmentsServiceClient _grpcClient) : IUserDepartmentLogic
    {
        public async Task<List<Guid>> GetUserDepartmentIds(Guid userId)
        {
            var cacheKey = $"user:{userId}:departments";

            var cached = await _cache.GetAsync<List<Guid>>(cacheKey);

            if(cached != null)
                return cached;

            var response = await _grpcClient.GetUserDepartmentsAsync(new GetUserDepartmentsRequest
            {
                UserId = userId.ToString()
            });

            var departments = response.DepartmentIds
                .Select(Guid.Parse)
                .ToList();

            await _cache.SetAsync(cacheKey, departments, TimeSpan.FromDays(7));

            return departments;
        }
    }
}
