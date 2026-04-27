using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Models;
using TechTrack.MaintenanceService.Schedule.Entities;
using TechTrack.MaintenanceService.Schedule.Interfaces;
using TechTrack.MaintenanceService.Schedule.Models.Requests;
using TechTrack.MaintenanceService.Schedule.Models.Responses;
using TechTrack.MaintenanceService.Shared.Implementations;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Exceptions;

namespace TechTrack.MaintenanceService.Schedule.Implementations
{
    public class ScheduleLogic(MaintenanceServiceDbContext dbContext, UserDepartmentLogic userDepartmentLogic) : IScheduleLogic
    {
        public async Task<AddScheduleRecordResponse> AddScheduleRecord(AddScheduleRecordRequest request, UserPermissionInfo userInfo)
        {
            var equipmentProjection = await dbContext.EquipmentsProjection.FirstOrDefaultAsync(e => e.Id == request.EquipmentId)
                ?? throw new NotFoundException("Оборудование с таким ID не найдено.");

            if (equipmentProjection.CompanyId != userInfo.CompanyId)
                throw new ForbiddenException("Вы должны быть сотрудником компании.");

            var userDepartments = await userDepartmentLogic.GetUserDepartmentIds(userInfo.UserId);
            if(!userDepartments.Contains(equipmentProjection.DepartmentId))
            {
                throw new ForbiddenException("У вас нет доступа к планированию ТО для техники этого отдела.");
            }

            var scheduleRecord = new MaintenanceSchedule
            {
                EquipmentId = request.EquipmentId,
                MaintenanceName = request.MaintenanceName,
                NotificationAdvanceDays = request.NotificationAdvanceDays,
                IntervalValue = request.IntervalValue,
                NextMaintenanceDate = request.NextMaintenanceDate,
                RecurrenceTypeId = request.RecurrenceTypeId,
                CreatorId = userInfo.UserId,
                ResponsibleUserId = request.ResponsibleUserId
            };

            var scheduledMaintenanceRecord = new Maintenance
            {
                MaintenanceScheduleRecord = scheduleRecord,
                MaintenanceStatusId = (int)MaintenanceStatusTypes.Scheduled,
                MaintenanceTypeId = (int)MaintenanceTypes.Preventive,
                ScheduledDate = scheduleRecord.NextMaintenanceDate,
                Name = request.MaintenanceName,
                EquipmentId = request.EquipmentId,
                ResponsibleUserId = request.ResponsibleUserId
            };

            await dbContext.AddAsync(scheduleRecord);
            await dbContext.AddAsync(scheduledMaintenanceRecord);
            await dbContext.SaveChangesAsync();

            return new AddScheduleRecordResponse
            (
                scheduleRecord.Id,
                scheduledMaintenanceRecord.Id
            );
        }

        private async Task<MaintenanceStatus> GetMaintenanceStatusByName(MaintenanceStatusTypes statusType)
        {
            var status = await dbContext.MaintenanceStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == (int)statusType);

            return status;
        }
    }
}
