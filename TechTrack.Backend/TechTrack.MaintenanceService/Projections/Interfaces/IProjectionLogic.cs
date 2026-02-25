using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Projections.Interfaces
{
    public interface IProjectionLogic
    {
        public Task<int> ProjectionSync(Guid departmentId, UserPermissionInfo userPermissionInfo);
    }
}
