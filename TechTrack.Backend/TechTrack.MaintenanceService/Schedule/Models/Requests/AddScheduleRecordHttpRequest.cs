namespace TechTrack.MaintenanceService.Schedule.Models.Requests
{
    public record AddScheduleRecordHttpRequest(int RecurrenceTypeId, string MaintenanceName, int IntervalValue, DateTime NextMaintenanceDate, Guid ResponsibleUserId, int NotificationAdvanceDays);
}
