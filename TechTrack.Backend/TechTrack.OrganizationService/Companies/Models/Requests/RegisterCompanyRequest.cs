using System.Security.Cryptography.X509Certificates;

namespace TechTrack.OrganizationService.Companies.Models.Requests
{
    public record RegisterCompanyRequest(string Name, string Address, Guid? CompanyHeadId);
}
