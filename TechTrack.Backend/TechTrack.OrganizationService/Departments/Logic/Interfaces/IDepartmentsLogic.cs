using TechTrack.OrganizationService.Departments.Models.Requests;
using TechTrack.OrganizationService.Departments.Models.Responses;

namespace TechTrack.OrganizationService.Departments.Logic.Interfaces
{
    public interface IDepartmentsLogic
    {
        public Task<CreateDepartmentResponse> CreateDepartmentAsync(Guid companyId,CreateDepartmentRequest request);

        public Task<GetAllDepartmentsResponse> GetAllDepartmentsAsync(Guid companyId);
    }
}
