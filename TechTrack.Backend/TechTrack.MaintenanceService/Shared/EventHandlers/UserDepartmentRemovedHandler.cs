using MassTransit;
using TechTrack.MaintenanceService.Shared.Interfaces;
using TechTrack.Shared.Events;

namespace TechTrack.MaintenanceService.Shared.EventHandlers
{
    public class UserDepartmentRemovedHandler(
        ICacheLogic _cacheLogic,
        IUserDepartmentCacheHelper _cacheHelper) : IConsumer<UserDepartmentRemoved>
    {
        public async Task Consume(ConsumeContext<UserDepartmentRemoved> context)
        {
            var message = context.Message;
            var key = _cacheHelper.GetUserDepartmentCacheKey(message.UserId);

            var departments = await _cacheLogic.GetAsync<List<Guid>>(key);

            if (departments == null)
                departments = new List<Guid>();

            if (!departments.Remove(message.DepartmentId))
                return;

            await _cacheLogic.SetAsync(key, departments, _cacheHelper.GetUserDepartmentCacheTTL());
        }
    }
}
