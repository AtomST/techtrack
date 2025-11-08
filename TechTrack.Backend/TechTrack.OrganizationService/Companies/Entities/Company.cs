using TechTrack.OrganizationService.Departments.Entities;

namespace TechTrack.OrganizationService.Companies.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime ConnectedAt { get; set; }

        public Guid? CompanyHeadId { get; set; }
        public IList<CompanyUser> UsersInfo { get; set; } = new List<CompanyUser>();
        public IList<Department> Departments { get; set; } = new List<Department>();
    }
}
