namespace TechTrack.OrganizationService.Departments.Models.Requests
{
    public record CreateDepartmentRequest(string Name, Guid? ResponsibleUserId);
}
