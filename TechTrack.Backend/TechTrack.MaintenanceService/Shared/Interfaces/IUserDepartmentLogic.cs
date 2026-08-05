namespace TechTrack.MaintenanceService.Shared.Interfaces
{
    public interface IUserDepartmentLogic
    {
        Task<List<Guid>> GetUserDepartmentIds(Guid userId);
    }
}
