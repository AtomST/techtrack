using MassTransit;
using TechTrack.MaintenanceService.Shared.Interfaces;
using TechTrack.Shared.Protos;

namespace TechTrack.MaintenanceService.Shared.Implementations
{
    public class UserDepartmentLogic(
        ICacheLogic _cache, 
        UserDepartmentsService.UserDepartmentsServiceClient _grpcClient,
        IUserDepartmentCacheHelper _cacheHelper) : IUserDepartmentLogic
    {
        public async Task<List<Guid>> GetUserDepartmentIds(Guid userId)
        {
            var cacheKey = _cacheHelper.GetUserDepartmentCacheKey(userId);

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

            await _cache.SetAsync(cacheKey, departments, _cacheHelper.GetUserDepartmentCacheTTL());

            return departments;
        }
    }

}
