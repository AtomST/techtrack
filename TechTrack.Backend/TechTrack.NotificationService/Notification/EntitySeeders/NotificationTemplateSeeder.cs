using TechTrack.NotificationService.Data;
using TechTrack.NotificationService.Notification.Entities;
using TechTrack.NotificationService.Notification.Models;
using TechTrack.Shared.Database;

namespace TechTrack.NotificationService.Notification.EntitySeeders
{
    public class NotificationTemplateSeeder(SharedDbSeeder<NotificationServiceDbContext> dbSeeder) : IEntitySeeder
    {
        public int Order => 2;

        private readonly string MAINTENANCECOMPLETED_MESSAGE_TEMPLATE =
@"Техническое обслуживание «{maintenanceName}» успешно завершено.

Дата выполнения: {completedAt}
Тип технического обслуживания: {maintenanceType}
Ответственный: {responsibleName}
Выполнено: {completedByName}";
        private readonly string MAINTENANCEREMINDER_MESSAGE_TEMPLATE =
@"Напоминаем вам о техническом обслуживании «{maintenanceName}».

Дата проведения: {scheduledDate}
Осталось: {days} дн.

Пожалуйста, подготовьте оборудование и ресурсы.";
        private readonly string MAINTENANCEOVERDUE_MESSAGE_TEMPLATE =
@"Техническое обслуживание «{maintenanceName}» не выполнено вовремя.

Плановая дата: {scheduledDate}
Ответственный: {responsibleName}

Рекомендуется выполнить обслуживание как можно скорее.";
        private readonly string CRITICALISSUEDETECTED_MESSAGE_TEMPLATE =
@"Обнаружена критическая проблема на оборудовании ({equipmentName}).

Проблема: {issueName}
Описание: {issueDescription}

Требуется немедленное вмешательство.";
        public async Task SeedAsync()
        {
            var data = new[]
            {
                new NotificationTemplate
                {
                    Id = 1,
                    NotificationTypeId = (int)NotificationTypes.MaintenanceCompleted,
                    TitleTemplate = @"{equipmentName} — ТО выполнено",
                    MessageTemplate = MAINTENANCECOMPLETED_MESSAGE_TEMPLATE,
                },
                new NotificationTemplate
                {
                    Id = 2,
                    NotificationTypeId = (int)NotificationTypes.MaintenanceOverdue,
                    TitleTemplate = @"{equipmentName} — ТО просрочено на {days} дн.",
                    MessageTemplate = MAINTENANCEOVERDUE_MESSAGE_TEMPLATE,
                },
                new NotificationTemplate
                {
                    Id = 3,
                    NotificationTypeId = (int)NotificationTypes.MaintenanceReminder,
                    TitleTemplate = @"{equipmentName} — запланировано ТО",
                    MessageTemplate = MAINTENANCEREMINDER_MESSAGE_TEMPLATE,
                },
                new NotificationTemplate
                {
                    Id = 4,
                    NotificationTypeId = (int)NotificationTypes.CriticalIssueDetected,
                    TitleTemplate = @"{equipmentName} - критическая проблема",
                    MessageTemplate = CRITICALISSUEDETECTED_MESSAGE_TEMPLATE
                }
            };

            await dbSeeder.SeedAsync(data, x => x.Id);
        }
    }
}
