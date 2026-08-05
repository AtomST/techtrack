namespace TechTrack.NotificationService.Notification.Logic.Mappers
{
    public interface ITemplateModelMapper<in T>
    {
        public Dictionary<string, string?> Map(T model);
    }
}
