using TechTrack.MaintenanceService.Maintenances.Entities;

namespace TechTrack.MaintenanceService.Maintenances.Models.Responses
{
    public record GetMaintenanceLogByEquipmentIdResponse(IEnumerable<Maintenance> MaintenanceLog);
}
