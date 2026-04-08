using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.OrganizationService.Departments.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.OrganizationService.Departments.Logic.Interfaces
{
    public interface IDepartmentsLogic
    {
        public Task<CreateDepartmentResponse> CreateDepartmentAsync(CreateDepartmentRequest request, UserPermissionInfo userInfo);

        public Task<GetAllDepartmentsResponse> GetAllDepartmentsAsync(UserPermissionInfo userInfo);

        public Task<GetFullDepartmentInfoResponse> GetFullDepartmentInfoAsync(Guid departmentId, UserPermissionInfo permissionInfo);
    }
}
