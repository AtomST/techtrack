using TechTrack.OrganizationService.Companies.Entities;
using TechTrack.OrganizationService.Companies.Models.Requests;
using TechTrack.OrganizationService.Companies.Models.Responses;
using TechTrack.Shared.Auth;

namespace TechTrack.OrganizationService.Companies.Logic.Interfaces
{
    public interface ICompaniesLogic
    {
        public Task<RegisterCompanyResponse> RegisterAsync(RegisterCompanyRequest companyDto);
        public Task AddEmployeeByEmailAsync(AddEmployeeByEmailRequest request, Guid companyId);
    }
}
