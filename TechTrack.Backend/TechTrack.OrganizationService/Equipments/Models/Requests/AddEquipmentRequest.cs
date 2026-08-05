namespace TechTrack.OrganizationService.Equipments.Models.Requests
{
    public class AddEquipmentRequest
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string? Desctiption { get; set; }
        public Guid? ResponsibleUserId { get; set; }
    }
}
