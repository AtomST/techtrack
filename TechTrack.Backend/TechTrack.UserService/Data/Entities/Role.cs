namespace TechTrack.UserService.Data.Entities
{
    public class Role
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;

        public IList<User> Users { get; set; } = new List<User>();
    }
}
