using TechTrack.MaintenanceService.Maintenances.Models.Requests;
using TechTrack.MaintenanceService.Maintenances.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.MaintenanceService.Maintenances.Interfaces
{
    public interface IMaintenanceLogic
    {
        public Task<AddMaintenanceResponse> AddMaintenance(AddMaintenanceRequest maintenance, UserPermissionInfo userInfo);
        public Task<GetMaintenanceLogByEquipmentIdResponse> GetMaintenanceLogByEquipment(Guid equipmentId, UserPermissionInfo userInfo);
        public Task CompleteMaintenanceAsync(CompleteMaintenanceRequest request, UserPermissionInfo userInfo);
    }
}
