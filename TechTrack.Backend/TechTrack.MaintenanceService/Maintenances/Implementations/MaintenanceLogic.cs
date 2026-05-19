using MassTransit;
using MassTransit.Middleware;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Models;
using TechTrack.MaintenanceService.Maintenances.Models.Requests;
using TechTrack.MaintenanceService.Maintenances.Models.Responses;
using TechTrack.MaintenanceService.Schedule.Models;
using TechTrack.MaintenanceService.Shared.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;

namespace TechTrack.MaintenanceService.Maintenances.Implementations
{
    public class MaintenanceLogic(MaintenanceServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<MaintenanceLogic> logger, IUserDepartmentLogic userDepartmentLogic) : IMaintenanceLogic
    {
        public async Task<AddMaintenanceResponse> AddMaintenance(AddMaintenanceRequest request, UserPermissionInfo userInfo)
        {
            var equipmentProjection = await dbContext.EquipmentsProjection.FirstOrDefaultAsync(e => e.Id == request.EquipmentId)
                ?? throw new NotFoundException("Оборудование с таким ID не найдено.");

            if (equipmentProjection.CompanyId != userInfo.CompanyId)
                throw new ForbiddenException("Вы должны быть сотрудником компании.");

            var existingIssues = await GetAndValidateExistingIssueIds(request.SolvedIssuesId, request.EquipmentId);

            var now = DateTime.UtcNow;
            var maintenanceType = await dbContext.MaintenanceTypes.FirstOrDefaultAsync(t => t.Id == (int)MaintenanceTypes.Repair);
            var maintenance = new Maintenance
            {
                Name = request.Name,
                Description = request.Desctiprion,
                EquipmentId = request.EquipmentId,
                CompletedAt = now,
                ResponsibleUserId = userInfo.UserId,
                MaintenanceTypeId = maintenanceType.Id,
                MaintenanceStatusId = (int)MaintenanceStatusTypes.Completed
            };
            await using var tx = await dbContext.Database.BeginTransactionAsync();

            await dbContext.MaintenanceLog.AddAsync(maintenance);
            await dbContext.SaveChangesAsync();

            var resolvedIssues = await ResolveIssues(existingIssues, maintenance.Id);

            var maxEquipmentStatusCode = await dbContext.Issues
                .AsNoTracking()
                .Where(i =>
                    i.EquipmentId == request.EquipmentId &&
                    i.IsResolved == false)
                .Select(i => (int?)i.StatusId)
                .MaxAsync() ?? 1;

            await publishEndpoint.Publish(new MaintenanceAdded
            {
                EquipmentId = request.EquipmentId,
                MaxIssueStatusCode = maxEquipmentStatusCode
            });

            await publishEndpoint.Publish(new MaintenanceCompletedEvent
            {
                CompletedAt = now,
                EquipmentId = request.EquipmentId,
                ScheduledByUserId = userInfo.UserId,
                CompletedByUserId = userInfo.UserId,
                MaintenanceId = maintenance.Id,
                MaintenanceName = request.Name,
                MaintenanceTypeName = maintenanceType.NameRu,
                EquipmentName = equipmentProjection.Name,
            });

            await dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return new AddMaintenanceResponse();

        }

        private async Task<int> ResolveIssues(IEnumerable<Guid> issuesId, Guid maintenanceId)
        {
            return await dbContext.Issues
                .Where(i => issuesId.Contains(i.Id))
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.IsResolved, true)
                    .SetProperty(i => i.ResolvedByMaintenanceId, maintenanceId));
        }

        private async Task<IEnumerable<Guid>> GetAndValidateExistingIssueIds(IEnumerable<Guid> issuesId, Guid equipmentId)
        {
            var issues = await dbContext.Issues
                .Where(i => issuesId.Contains(i.Id) && i.EquipmentId == equipmentId)
                .Select(i => i.Id)
                .ToListAsync();

            logger.LogInformation($"{issues[0]}");
            logger.LogInformation($"{issues.GetType()}");
            return issues;
        }

        public async Task<GetMaintenanceLogByEquipmentIdResponse> GetMaintenanceLogByEquipment(Guid equipmentId, UserPermissionInfo userInfo)
        {
            var equipmentProjection = await dbContext.EquipmentsProjection.FirstOrDefaultAsync(e => e.Id == equipmentId)
                ?? throw new NotFoundException("Оборудование с таким ID не найдено.");

            if (equipmentProjection.CompanyId != userInfo.CompanyId)
                throw new ForbiddenException("Вы должны быть сотрудником компании.");

            var maintenanceLog = await dbContext.MaintenanceLog
                .AsNoTracking()
                .Where(m => m.EquipmentId == equipmentId)
                .ToListAsync();

            return new GetMaintenanceLogByEquipmentIdResponse
            (
                maintenanceLog
            );
        }

        public async Task CompleteMaintenanceAsync(CompleteMaintenanceRequest request, UserPermissionInfo userInfo)
        {
            var maintenanceRecord = await dbContext.MaintenanceLog
                .FirstOrDefaultAsync(m => m.Id == request.MaintenanceId)
                ?? throw new NotFoundException("ТО с таким ID не найдено.");

            var equipmentProjection = await dbContext.EquipmentsProjection.AsNoTracking().FirstOrDefaultAsync(p => p.Id == maintenanceRecord.EquipmentId);
            if (equipmentProjection.CompanyId != userInfo.CompanyId)
                throw new ForbiddenException("У вас нет доступа к ТО этой компании.");

            if(maintenanceRecord.MaintenanceStatusId == (int)MaintenanceStatusTypes.Completed)
                throw new InvalidInputException("ТО уже проведено.");

            await UpdateCompletedMaintenanceProperties(
                maintenanceRecord, 
                request, 
                userInfo.UserId
            );

            var scheduleTemplate = await dbContext.MaintenanceSchedule
                .FirstOrDefaultAsync(x => x.Id == maintenanceRecord.MaintenanceScheduleRecordId);

            var nextMaintenanceDate = CalculateNextMaintenanceDate(
                scheduleTemplate.NextMaintenanceDate,
                scheduleTemplate.RecurrenceTypeId,
                scheduleTemplate.IntervalValue
            );

            scheduleTemplate.NextMaintenanceDate = nextMaintenanceDate;

            var newScheduledMaintenance = new Maintenance
            {
                Name = scheduleTemplate.MaintenanceName,
                MaintenanceScheduleRecord = scheduleTemplate,
                MaintenanceStatusId = (int)MaintenanceStatusTypes.Scheduled,
                MaintenanceTypeId = (int)MaintenanceTypes.Preventive,
                EquipmentId = scheduleTemplate.EquipmentId,
                ResponsibleUserId = scheduleTemplate.ResponsibleUserId,
                ScheduledDate = nextMaintenanceDate
            };
            await dbContext.AddAsync(newScheduledMaintenance);
            await dbContext.SaveChangesAsync();
            return;
        }

        private async Task UpdateCompletedMaintenanceProperties(Maintenance record, CompleteMaintenanceRequest newProperties, Guid completedByUserId)
        {
            record.MaintenanceStatusId = (int)MaintenanceStatusTypes.Completed;
            record.Description = newProperties.Description;
            record.CompletedByUserId = completedByUserId;
            record.CompletedAt = DateTime.UtcNow;

            if(newProperties.MaintenanceTypeId != null)
            {
                var type = await dbContext.MaintenanceTypes.FirstOrDefaultAsync(t => t.Id == newProperties.MaintenanceTypeId)
                    ?? throw new InvalidInputException("MaintenanceType с таким ID не существует.");

                record.MaintenanceTypeId = type.Id;
            }
        }

        private DateTime CalculateNextMaintenanceDate(DateTime pastMaintenanceDate, int recurrenceTypeId, int intervalValue)
        {
            DateTime nextMaintenanceDate;

            switch (recurrenceTypeId)
            {
                case (int)ScheduleRecurrenceTypes.Day:
                    nextMaintenanceDate = pastMaintenanceDate.AddDays(intervalValue);
                    break;
                case (int)ScheduleRecurrenceTypes.Week:
                    nextMaintenanceDate = pastMaintenanceDate.AddDays(7 * intervalValue);
                    break;
                case (int)ScheduleRecurrenceTypes.Month:
                    nextMaintenanceDate = pastMaintenanceDate.AddMonths(intervalValue);
                    break;
                default: throw new NotImplementedException();
            }

            return nextMaintenanceDate;
        }

        public async Task<GetAllDepartmentMaintenancesResponse> GetAllDepartmentMaintenances(Guid departmentId, UserPermissionInfo userInfo)
        {
            logger.LogInformation(userInfo.Role);
            if(userInfo.Role != Roles.CompanyHead && userInfo.Role != Roles.Admin)
            {
                var userDepartmentLogicResponse = await userDepartmentLogic.GetUserDepartmentIds(userInfo.UserId);
                var userDepartments = userDepartmentLogicResponse.ToHashSet();

                if (!userDepartments.Contains(departmentId))
                    throw new ForbiddenException("У вас нет доступа к журналу ТО этого отдела.");
            }

            var maintenanceLog = await dbContext.EquipmentsProjection
                .AsNoTracking()
                .Where(e => e.DepartmentId == departmentId)
                .Join(
                    dbContext.MaintenanceLog,
                    e => e.Id,
                    m => m.EquipmentId,
                    (e, m) => m
               )
            .ToListAsync();

            return new GetAllDepartmentMaintenancesResponse { MaintenanceLog = maintenanceLog };
        }
    }
}
