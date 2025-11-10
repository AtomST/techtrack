namespace TechTrack.Shared.Auth
{
    public class UserPermissionInfo
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string CompanyId { get; set; }
    }
}
