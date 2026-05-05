namespace TechTrack.OrganizationService.Companies.Models
{
    public class EmployeeInfo
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public Guid? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
