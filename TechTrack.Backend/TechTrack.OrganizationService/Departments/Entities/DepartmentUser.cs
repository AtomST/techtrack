namespace TechTrack.OrganizationService.Departments.Entities
{
    public class DepartmentUser
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
