using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Logic.Mappers;
using TechTrack.NotificationService.Notification.Logic.Mappers.Models;
using TechTrack.NotificationService.Notification.Models;
using TechTrack.Shared.Events;

namespace TechTrack.NotificationService.Notification.Logic.EventHandlers
{
    public class CriticalIssueDetectedHandler(
        NotificationServiceDbContext dbContext, 
        ITemplateModelMapper<CriticalIssueDetectedMapperModel> modelMapper,
        TemplateDictionaryMapper templateMapper) : IConsumer<CriticalIssueDetectedEvent>
    {
        private readonly int _notificationTypeId = (int)NotificationTypes.CriticalIssueDetected;

        public async Task Consume(ConsumeContext<CriticalIssueDetectedEvent> context)
        {
            var message = context.Message;
            var now = DateTime.UtcNow;

            var recipientId = DetermineRecipient(message.ResponsibleUserId, message.ManagerId, message.DepartmentHeadId);
            if (!recipientId.HasValue)
                return;

            var recipientContacts = await dbContext.UserContacts
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == recipientId);

            var notificationTemplate = await dbContext.NotificationTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NotificationTypeId == _notificationTypeId);

            var mapperModel = new CriticalIssueDetectedMapperModel
            {
                EquipmentName = message.EquipmentName,
                IssueName = message.IssueName,
                IssueDescription = message.IssueDescription
            };

            var modelMapperValues = modelMapper.Map(mapperModel);

            var mappedTitle = templateMapper.Map(notificationTemplate.TitleTemplate, modelMapperValues);
            var mappedMessage = templateMapper.Map(notificationTemplate.MessageTemplate, modelMapperValues);

            var notification = new Entities.Notification
            {
                NotificationTypeId = _notificationTypeId,
                UserId = recipientId.Value,
                CreatedAt = now,
                Title = mappedTitle,
                Message = mappedMessage,
                RelatedEntityId = message.EquipmentId
            };

            await dbContext.AddAsync(notification);
            await dbContext.SaveChangesAsync();
        }

        private Guid? DetermineRecipient(Guid? ResponsibleUserId, Guid? ManagerId, Guid? DepartmentHeadId)
        {
            return ResponsibleUserId ?? ManagerId ?? DepartmentHeadId;
        }
    }
}
