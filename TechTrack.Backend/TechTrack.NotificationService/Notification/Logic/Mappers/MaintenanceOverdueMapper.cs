using TechTrack.NotificationService.Notification.Logic.Mappers.Models;

namespace TechTrack.NotificationService.Notification.Logic.Mappers
{
    public class MaintenanceOverdueMapper : ITemplateModelMapper<MaintenanceOverdueMapperModel>
    {
        public Dictionary<string, string?> Map(MaintenanceOverdueMapperModel model)
        {
            return new()
            {
                ["maintenanceName"] = model.MaintenanceName,
                ["equipmentName"] = model.EquipmentName,
                ["responsibleName"] = model.ResponsibleUserName,
                ["scheduledDate"] = model.ScheduledDate.ToString()
            };
        }
    }
}
