using TechTrack.MaintenanceService.Maintenances.Entities;

namespace TechTrack.MaintenanceService.Maintenances.Models.Responses
{
    public class GetAllDepartmentMaintenancesResponse
    {
        public IList<Maintenance> MaintenanceLog { get; set; }
    }
}
