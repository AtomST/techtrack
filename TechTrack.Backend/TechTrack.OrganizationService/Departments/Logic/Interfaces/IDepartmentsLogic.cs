using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.OrganizationService.Departments.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.OrganizationService.Departments.Logic.Interfaces
{
    public interface IDepartmentsLogic
    {
        public Task<CreateDepartmentResponse> CreateDepartmentAsync(Guid companyId,CreateDepartmentRequest request);

        public Task<GetAllDepartmentsResponse> GetAllDepartmentsAsync(Guid companyId);

        public Task<GetFullDepartmentInfoResponse> GetFullDepartmentInfoAsync(Guid departmentId, UserPermissionInfo permissionInfo);
    }
}
