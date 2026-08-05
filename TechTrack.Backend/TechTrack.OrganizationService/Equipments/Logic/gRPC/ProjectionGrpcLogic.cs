using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService.Equipments.Logic.gRPC
{
    public class ProjectionGrpcLogic : ProjectionService.ProjectionServiceBase
    {
        private readonly OrganizationServiceDbContext _dbContext;
        public ProjectionGrpcLogic(OrganizationServiceDbContext dbContext)
        {
            _dbContext = dbContext; 
        }
        public override async Task<GetDepartmentEquipmentsResponse> GetDepartmentEquipments(GetDepartmentEquipmentsRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                throw new RpcException(new Status
                (
                    StatusCode.InvalidArgument,
                    "Неверный формат UserId."
                ));

            if (!Guid.TryParse(request.DepartmentId, out var departmentId))
                throw new RpcException(new Status
                (
                    StatusCode.InvalidArgument,
                    "Неверный формат DepartmentId."
                ));
            if (request.UserRole == Roles.Undefined ||
                request.UserRole == Roles.Employee)
                throw new RpcException(new Status
                    (
                        StatusCode.PermissionDenied,
                        "У вас недостаточно прав для синхронизации данных."
                    ));

            var department = _dbContext.Departments
                .AsNoTracking()
                .Include(d => d.Equipments)
                .Where(d => d.Id == departmentId)
                .FirstOrDefault() 
                ?? throw new RpcException(new Status
                (
                    StatusCode.NotFound,
                    "Отдел с таким Id не найден."
                ));

            var isUserInCompany = _dbContext.CompanyUser
                .AsNoTracking()
                .Where(u => u.CompanyId == department.CompanyId && u.UserId == userId)
                .FirstOrDefault()
                ?? throw new RpcException(new Status
                (
                    StatusCode.PermissionDenied,
                    "Проводить синхронизацию могут только сотрудники компании."
                ));
            var response = new GetDepartmentEquipmentsResponse()
            {
                DepartmentId = department.Id.ToString(),
                CompanyId = department.CompanyId.ToString()
            };
            foreach(var equipment in department.Equipments)
            {
                response.Equipments.Add(new Equipment
                {
                    Id = equipment.Id.ToString(),
                    Name = equipment.Name,
                });
            }
            return response;
        }
    }
}
