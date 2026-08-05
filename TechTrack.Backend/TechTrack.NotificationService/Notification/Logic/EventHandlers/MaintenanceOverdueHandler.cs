using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Entities;
using TechTrack.NotificationService.Notification.Logic.Mappers;
using TechTrack.NotificationService.Notification.Logic.Mappers.Models;
using TechTrack.NotificationService.Notification.Models;
using TechTrack.Shared.Events;

namespace TechTrack.NotificationService.Notification.Logic.EventHandlers
{
    public class MaintenanceOverdueHandler(
        NotificationServiceDbContext dbContext,
        ITemplateModelMapper<MaintenanceOverdueMapperModel> modelMapper,
        TemplateDictionaryMapper templateMapper) : IConsumer<MaintenanceOverdueEvent>
    {
        private readonly int _notificationTypeId = (int)NotificationTypes.MaintenanceOverdue;
        public async Task Consume(ConsumeContext<MaintenanceOverdueEvent> context)
        {
            var message = context.Message;
            var now = DateTime.UtcNow;

            var notificationTemplate = await dbContext.NotificationTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NotificationTypeId == _notificationTypeId);

            var responsibleUserContacts = await dbContext.UserContacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == message.ResponsibleUserId);

            var recipients = new List<Guid>() { message.ScheduledByUserId};
            if (message.ResponsibleUserId.HasValue && message.ScheduledByUserId != message.ResponsibleUserId)
                recipients.Add(message.ResponsibleUserId.Value);

            var mapperModel = new MaintenanceOverdueMapperModel
            {
                MaintenanceName = message.MaintenanceName,
                EquipmentName = message.EquipmentName,
                ScheduledDate = message.ScheduledDate,
                ResponsibleUserName = responsibleUserContacts?.Name
            };

            var modelMapperValues = modelMapper.Map(mapperModel);

            var mappedTitle = templateMapper.Map(notificationTemplate.TitleTemplate, modelMapperValues);
            var mappedMessage = templateMapper.Map(notificationTemplate.MessageTemplate, modelMapperValues);

            var notifications = recipients.Select(userId => new Entities.Notification
            {
                NotificationTypeId = _notificationTypeId,
                UserId = userId,
                CreatedAt = now,
                Title = mappedTitle,
                Message = mappedMessage,
                RelatedEntityId = message.MaintenanceId
            });

            await dbContext.AddRangeAsync(notifications);
            await dbContext.SaveChangesAsync();
        }
    }
}
