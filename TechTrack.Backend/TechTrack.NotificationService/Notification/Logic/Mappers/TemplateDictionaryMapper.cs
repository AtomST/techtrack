using System.Text;

namespace TechTrack.NotificationService.Notification.Logic.Mappers
{
    public class TemplateDictionaryMapper
    {
        public string Map(string template, Dictionary<string, string> pairs)
        {
            StringBuilder sb = new StringBuilder(template);
            foreach (var pair in pairs)
            {
                var replaceValue = pair.Value ?? "-";
                sb.Replace($"{{{pair.Key}}}", replaceValue);
            }
            return sb.ToString();
        }
    }
}
