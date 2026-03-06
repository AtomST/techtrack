using MassTransit;
using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Data.SharedEntities;
using TechTrack.MaintenanceService.Issues.Entities;
using TechTrack.MaintenanceService.Issues.Logic.Interfaces;
using TechTrack.MaintenanceService.Issues.Logic.Models.Request;
using TechTrack.MaintenanceService.Issues.Logic.Models.Responses;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Events;
using TechTrack.Shared.Exceptions;
using TechTrack.Shared.Protos;

namespace TechTrack.MaintenanceService.Issues.Logic.Implementations
{
    public class IssuesLogic : IIssuesLogic
    {
        private readonly MaintenanceServiceDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public IssuesLogic(MaintenanceServiceDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<CreateIssueResponse> CreateIssueAsync(CreateIssueRequest request, UserPermissionInfo userInfo)
        {
            var equipmentProjection = await GetEquipmentProjectionWithCompanyCheck(request.EquipmentId, Guid.Parse(userInfo.CompanyId));

            var equipmentStatus = await _dbContext.EquipmentStatuses.FirstOrDefaultAsync(s => s.Id == request.StatusId)
                ?? throw new NotFoundException("Статус с таким ID не найден.");

            var issue = new Issue
            {
                Name = request.Name,
                StatusId = request.StatusId,
                Description = request.Desctiption,
                EquipmentStatus = equipmentStatus,
                CreatedAt = DateTime.UtcNow,
                CreatorId = userInfo.UserId,
                EquipmentId = request.EquipmentId,
            };

            _dbContext.Issues.Add(issue);
            await _dbContext.SaveChangesAsync();
            
            await _publishEndpoint.Publish(new IssueRegistred
            {
                EquipmentId = request.EquipmentId,
                StatusId = request.StatusId,
            });

            return new CreateIssueResponse(issue.Id);

        }

        public async Task<GetAllIssuesResponse> GetAllIssues(Guid equipmentId, UserPermissionInfo permissionInfo)
        {
            await GetEquipmentProjectionWithCompanyCheck(equipmentId, Guid.Parse(permissionInfo.CompanyId));

            var issues = await _dbContext.Issues
                .AsNoTracking()
                .Where(i => i.EquipmentId == equipmentId)
                .ToListAsync();

            return new GetAllIssuesResponse
            {
                Issues = issues
            };
        }
        
        private async Task<EquipmentProjection> GetEquipmentProjectionWithCompanyCheck(Guid equipmentId, Guid companyId)
        {
            var equipmentProjection = await _dbContext.EquipmentsProjection.FirstOrDefaultAsync(e => e.Id == equipmentId)
                ?? throw new NotFoundException("Оборудование с таким ID не найдено.");

            if (equipmentProjection.CompanyId != companyId)
                throw new ForbiddenException("Вы должны быть сотрудником компании.");

            return equipmentProjection;
        }
    }
}
