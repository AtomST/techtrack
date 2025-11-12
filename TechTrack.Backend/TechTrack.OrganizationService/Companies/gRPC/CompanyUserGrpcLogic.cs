using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TechTrack.OrganizationService.Data;
using TechTrack.Shared.Protos;

namespace TechTrack.OrganizationService.Companies.gRPC
{
    public class CompanyUserGrpcLogic : CompanyUserService.CompanyUserServiceBase
    {
        private readonly OrganizationServiceDbContext _dbContext;

        public CompanyUserGrpcLogic(OrganizationServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<IsUserInCompanyResponse> IsUserInCompany(IsUserInCompanyRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Неверный формат UserId"));

            if (!Guid.TryParse(request.CompanyId, out var companyId))
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Неверный формат CompanyId"));

            var isUserInCompany = await _dbContext.CompanyUser.AnyAsync(u => u.UserId == userId && u.CompanyId == companyId);
            return new IsUserInCompanyResponse
            { 
                IsUserInCompany = isUserInCompany 
            };
        }
    }
}
