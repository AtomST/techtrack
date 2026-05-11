using TechTrack.OrganizationService.Companies.Models;
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
        public Task AddEmployeeByIdAsync(Guid departmentId, UserPermissionInfo permissionInfo, AddEmployeeByIdRequest request);

        public Task<IList<EmployeeInfo>> GetAllDepartmentEmployees(Guid departmentId, UserPermissionInfo userInfo);

        public Task<GetAllDepartmentsResponse> GetAllUserDepartmentsAsync(UserPermissionInfo userInfo);
    }
}
