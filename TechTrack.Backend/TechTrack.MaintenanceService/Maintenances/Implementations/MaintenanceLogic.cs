using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Maintenances.Entities;
using TechTrack.MaintenanceService.Maintenances.Interfaces;
using TechTrack.MaintenanceService.Maintenances.Models.Requests;
using TechTrack.MaintenanceService.Maintenances.Models.Responses;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;

namespace TechTrack.MaintenanceService.Maintenances.Implementations
{
    public class MaintenanceLogic(MaintenanceServiceDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<MaintenanceLogic> logger) : IMaintenanceLogic
    {
        public async Task<AddMaintenanceResponse> AddMaintenance(AddMaintenanceRequest request, UserPermissionInfo userInfo)
        {
            var equipmentProjection = await dbContext.EquipmentsProjection.FirstOrDefaultAsync(e => e.Id == request.EquipmentId)
                ?? throw new NotFoundException("Оборудование с таким ID не найдено.");

            if (equipmentProjection.CompanyId != Guid.Parse(userInfo.CompanyId))
                throw new ForbiddenException("Вы должны быть сотрудником компании.");

            var existingIssues = await GetAndValidateExistingIssueIds(request.SolvedIssuesId, request.EquipmentId);

            var maintenance = new Maintenance
            {
                Name = request.Name,
                Description = request.Desctiprion,
                EquipmentId = request.EquipmentId,
                CreatedAt = DateTime.UtcNow,
                CreatorId = userInfo.UserId
            };

            await dbContext.MaintenanceLog.AddAsync(maintenance);
            await dbContext.SaveChangesAsync();
            var resolvedIssues = ResolveIssues(existingIssues, maintenance.Id);

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

            await dbContext.SaveChangesAsync();

            return new AddMaintenanceResponse();


        }

        private async Task<int> ResolveIssues(IEnumerable<Guid> issuesId, Guid maintenanceId)
        {
            //Заставить работать ExecuteUpdate с коллекцией
            logger.LogInformation($"MaintenanceId: {maintenanceId}");

            var idsArray = issuesId.ToArray(); // Guid[]
            var maintenanceIdParam = new NpgsqlParameter("maintenanceId", maintenanceId);
            var idsParam = new NpgsqlParameter("ids", NpgsqlDbType.Array | NpgsqlDbType.Uuid)
            {
                Value = idsArray
            };

            return await dbContext.Database.ExecuteSqlRawAsync(@"
                UPDATE issues 
                SET 
                    is_resolved = TRUE,
                    resolved_by_maintenance_id = @maintenanceId
                WHERE id = ANY(@ids)",
                maintenanceIdParam, idsParam);
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
    }
}
