using TechTrack.OrganizationService.Equipments.Models.Requests;
using TechTrack.OrganizationService.Equipments.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.OrganizationService.Equipments.Logic.Interfaces
{
    public interface IEquipmentsLogic
    {
        public Task<AddEquipmentResponse> AddEquipmentAsync(Guid departmentId, AddEquipmentRequest request, UserPermissionInfo userInfo);
        public Task<GetAllEquipmentsResponse> GetAllEquipmentsAsync(Guid departmentId, UserPermissionInfo userPermissionInfo);
    }
}
