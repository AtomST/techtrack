using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using TechTrack.MaintenanceService.Shared.Implementations;
using TechTrack.MaintenanceService.Shared.Interfaces;
using TechTrack.Shared.Events;

namespace TechTrack.MaintenanceService.Shared.EventHandlers
{
    public class UserDepartmentAddedHandler(
        ICacheLogic _cacheLogic,
        IUserDepartmentCacheHelper _cacheHelper) : IConsumer<UserDepartmentAdded>
    {
        public async Task Consume(ConsumeContext<UserDepartmentAdded> context)
        {
            var message = context.Message;
            var key = _cacheHelper.GetUserDepartmentCacheKey(message.UserId);

            var departments = await _cacheLogic.GetAsync<List<Guid>>(key);

            if (departments == null)
                departments = new List<Guid>();

            if (departments.Contains(message.DepartmentId))
                return;

            departments.Add(message.DepartmentId);
            await _cacheLogic.SetAsync(key, departments, _cacheHelper.GetUserDepartmentCacheTTL());
        }
    }
}
