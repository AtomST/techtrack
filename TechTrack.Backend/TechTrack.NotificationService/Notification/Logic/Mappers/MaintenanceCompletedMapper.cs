using TechTrack.NotificationService.Notification.Logic.Mappers.Models;

namespace TechTrack.NotificationService.Notification.Logic.Mappers
{
    public class MaintenanceCompletedMapper : ITemplateModelMapper<MaintenanceCompletedMapperModel>
    {
        public Dictionary<string, string?> Map(MaintenanceCompletedMapperModel model)
        {
            return new()
            {
                ["maintenanceName"] = model.MaintenanceName,
                ["equipmentName"] =
                    model.EquipmentName,
                ["completedAt"] = model.CompletedAt.ToString(),
                ["maintenanceType"] = model.MaintenanceTypeName,
                ["responsibleName"] = model.ResponsibleUserName,
                ["completedByName"] = model.CompletedByName
            };
        }
    }
}
