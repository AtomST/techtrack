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
            //var grpcResponse = await _projectionServiceClient.GetDepartmentEquipmentsAsync(
            //    new GetDepartmentEquipmentsRequest
            //    {
            //        UserId = userPermissionInfo.UserId.ToString(),
            //        DepartmentId = departmentId.ToString(),
            //        UserRole = userPermissionInfo.Role,
            //    });

            //if (grpcResponse.Equipments.Count == 0)
            //    return 0;

            //int result = await _dbContext.EquipmentsProjection
            //    .UpsertRange(grpcResponse.Equipments.Select
            //    (
            //        e => new EquipmentProjection
            //        {
            //            Id = Guid.Parse(e.Id),
            //            DepartmentId = Guid.Parse(grpcResponse.DepartmentId),
            //            CompanyId = Guid.Parse(grpcResponse.CompanyId),
            //        }
            //    )).RunAsync();

            //return result;
            var grpcResponse = await _projectionServiceClient.GetDepartmentEquipmentsAsync(
        new GetDepartmentEquipmentsRequest
        {
            UserId = userPermissionInfo.UserId.ToString(),
            DepartmentId = departmentId.ToString(),
            UserRole = userPermissionInfo.Role,
        });

            if (grpcResponse.Equipments.Count == 0)
                return 0;

            var incoming = grpcResponse.Equipments
                .Select(e => new EquipmentProjection
                {
                    Id = Guid.Parse(e.Id),
                    DepartmentId = Guid.Parse(grpcResponse.DepartmentId),
                    CompanyId = Guid.Parse(grpcResponse.CompanyId),
                })
                .ToList();

            var ids = incoming.Select(x => x.Id).ToList();

            var existing = await _dbContext.EquipmentsProjection
                .Where(x => ids.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var toInsert = new List<EquipmentProjection>();

            foreach (var item in incoming)
            {
                if (existing.TryGetValue(item.Id, out var dbEntity))
                {
                    if (dbEntity.DepartmentId != item.DepartmentId ||
                        dbEntity.CompanyId != item.CompanyId)
                    {
                        dbEntity.DepartmentId = item.DepartmentId;
                        dbEntity.CompanyId = item.CompanyId;
                    }
                }
                else
                {
                    toInsert.Add(item);
                }
            }

            if (toInsert.Count > 0)
                await _dbContext.EquipmentsProjection.AddRangeAsync(toInsert);

            await _dbContext.SaveChangesAsync();

            return incoming.Count;
        }
    }
}
