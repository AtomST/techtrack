namespace TechTrack.AuthService.Data.Entities
{
    public class UserInfoCache
    {
        public Guid UserId { get; set; }
        public Guid? CompanyId { get; set; }
        public string RoleName { get; set; } = "Undefined";

        public UserCredentials? UserCredentials {  get; set; }
    }
}
