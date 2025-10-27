namespace TechTrack.UserService.Data.Entities
{
    public class User
    {
        public Guid Id {  get; set; }
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
