using TechTrack.OrganizationService.Companies.Entities;
using TechTrack.OrganizationService.Equipments.Entities;

namespace TechTrack.OrganizationService.Departments.Entities
{
    public class Department
    {
        public Guid Id {  get; set; }
        public string Name { get; set; }

        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public Guid? ResponsibleUserId { get; set; }

        public IList<Equipment> Equipments { get; set; } = new List<Equipment>();
        public IList<DepartmentUser> DepartmentEmployees { get; set; } = new List<DepartmentUser>();
    }
}
