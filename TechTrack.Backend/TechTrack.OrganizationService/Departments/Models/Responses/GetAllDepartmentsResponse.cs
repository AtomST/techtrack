using TechTrack.OrganizationService.Departments.Entities;

namespace TechTrack.OrganizationService.Departments.Models.Responses
{
    public class GetAllDepartmentsResponse
    {
        public IList<Department> Departments { get; set; }
    }
}
