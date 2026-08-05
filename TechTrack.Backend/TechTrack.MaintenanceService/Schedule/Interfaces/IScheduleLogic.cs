using TechTrack.MaintenanceService.Schedule.Models.Requests;
using TechTrack.MaintenanceService.Schedule.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Schedule.Interfaces
{
    public interface IScheduleLogic
    {
        public Task<AddScheduleRecordResponse> AddScheduleRecord(AddScheduleRecordRequest request, UserPermissionInfo userInfo);
    }
}
