using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService.Departments.Logic.gRPC
{
    public class GetUserDepartmentsGrpcLogic(OrganizationServiceDbContext _dbContext) : UserDepartmentsService.UserDepartmentsServiceBase
    {
        public override async Task<GetUserDepartmentsResponse> GetUserDepartments(GetUserDepartmentsRequest request, ServerCallContext context)
        {
            var userId = Guid.Parse(request.UserId);

            var departments = await _dbContext.DepartmentUsers
                .Where(x => x.UserId == userId)
                .Select(x => x.DepartmentId.ToString())
                .ToListAsync();

            return new GetUserDepartmentsResponse
            {
                DepartmentIds = { departments }
            };
        }
    }
}
