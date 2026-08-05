using TechTrack.OrganizationService.Equipments.Entities;

namespace TechTrack.OrganizationService.Equipments.Models.Responses
{
    public class GetAllEquipmentsResponse
    {
        public IList<Equipment> Equipments { get; set; }
    }
}
