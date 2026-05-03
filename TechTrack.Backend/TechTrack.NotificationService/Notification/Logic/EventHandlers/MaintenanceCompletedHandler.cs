using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Entities;
using TechTrack.NotificationService.Notification.Logic.Mappers;
using TechTrack.NotificationService.Notification.Logic.Mappers.Models;
using TechTrack.NotificationService.Notification.Models;
using TechTrack.Shared.Events;

namespace TechTrack.NotificationService.Notification.Logic.EventHandlers
{
    public class MaintenanceCompletedHandler(NotificationServiceDbContext dbContext, ITemplateModelMapper<MaintenanceCompletedMapperModel> modelMapper, TemplateDictionaryMapper templateMapper) : IConsumer<MaintenanceCompletedEvent>
    {
        private readonly int _notificationTypeId = (int)NotificationTypes.MaintenanceCompleted;

        public async Task Consume(ConsumeContext<MaintenanceCompletedEvent> context)
        {
            var message = context.Message;

            if (message.ScheduledByUserId == null &&
                (message.ResponsibleUserId == null || message.ResponsibleUserId == message.CompletedByUserId))
                return;

            var notificationTemplate = await dbContext.NotificationTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NotificationTypeId == _notificationTypeId);

            var completedByUserContacts = await dbContext.UserContacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == message.CompletedByUserId);

            var responsibleUserContacts = await dbContext.UserContacts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == message.ResponsibleUserId);

            var mapperModel = new MaintenanceCompletedMapperModel
            {
                MaintenanceName = message.MaintenanceName,
                EquipmentName = message.EquipmentName,
                MaintenanceTypeName = message.MaintenanceTypeName,
                CompletedAt = message.CompletedAt,
                CompletedByName = completedByUserContacts?.Name,
                ResponsibleUserName = responsibleUserContacts?.Name
            };

            var modelMapperValues = modelMapper.Map(mapperModel);

            var mappedTitle = templateMapper.Map(notificationTemplate.TitleTemplate, modelMapperValues);
            var mappedMessage = templateMapper.Map(notificationTemplate.MessageTemplate, modelMapperValues);

            var recipientId = message.ScheduledByUserId ?? message.CompletedByUserId;
            var notification = new Entities.Notification
            {
                NotificationTypeId = _notificationTypeId,
                UserId = recipientId,
                CreatedAt = DateTime.UtcNow,
                Title = mappedTitle,
                Message = mappedMessage,
                RelatedEntityId = message.MaintenanceId
            };

            await dbContext.AddAsync(notification);
            await dbContext.SaveChangesAsync();
        }
    }
}
