using TechTrack.NotificationService.Notification.Logic.Mappers.Models;

namespace TechTrack.NotificationService.Notification.Logic.Mappers
{
    public class CriticalIssueDetectedMapper : ITemplateModelMapper<CriticalIssueDetectedMapperModel>
    {
        public Dictionary<string, string?> Map(CriticalIssueDetectedMapperModel model)
        {
            return new()
            {
                ["equipmentName"] = model.EquipmentName,
                ["issueName"] = model.IssueName,
                ["issueDescription"] = model.IssueDescription
            };
        }
    }
}
