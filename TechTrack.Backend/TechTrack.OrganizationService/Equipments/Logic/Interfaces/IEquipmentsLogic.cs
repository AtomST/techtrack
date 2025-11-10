using TechTrack.OrganizationService.Equipments.Models.Requests;
using TechTrack.OrganizationService.Equipments.Models.Responses;

namespace TechTrack.OrganizationService.Equipments.Logic.Interfaces
{
    public interface IEquipmentsLogic
    {
        public Task<AddEquipmentResponse> AddEquipmentAsync(Guid departmentId, AddEquipmentRequest request);
    }
}
