using Microsoft.EntityFrameworkCore;
using TechTrack.MaintenanceService.Data;
using TechTrack.MaintenanceService.Data.SharedEntities;
using TechTrack.MaintenanceService.Projections.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Protos;

namespace TechTrack.MaintenanceService.Projections.Implementations
{
    public class ProjectionLogic : IProjectionLogic
    {
        private readonly ProjectionService.ProjectionServiceClient _projectionServiceClient;
        private readonly MaintenanceServiceDbContext _dbContext;
        public ProjectionLogic(ProjectionService.ProjectionServiceClient projectionClient, MaintenanceServiceDbContext dbContext)
        {
            _projectionServiceClient = projectionClient;
            _dbContext = dbContext;
        }
        public async Task<int> ProjectionSync(Guid departmentId, UserPermissionInfo userPermissionInfo)
        {
            var grpcResponse = await _projectionServiceClient.GetDepartmentEquipmentsAsync(
                new GetDepartmentEquipmentsRequest
                {
                    UserId = userPermissionInfo.UserId.ToString(),
                    DepartmentId = departmentId.ToString(),
                    UserRole = userPermissionInfo.Role,
                });

            if (grpcResponse.Equipments.Count == 0)
                return 0;

            _dbContext.EquipmentsProjection
                .UpsertRange(grpcResponse.Equipments.Select
                (
                    e => new EquipmentProjection 
                    {
                        Id = Guid.Parse(e.Id),
                        DepartmentId = Guid.Parse(grpcResponse.DepartmentId),
                        CompanyId = Guid.Parse(grpcResponse.CompanyId),
                    }
                ));

            int res = await _dbContext.SaveChangesAsync();

            return grpcResponse.Equipments.Count;
        }
    }
}
