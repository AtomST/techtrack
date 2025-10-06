using System.ComponentModel.DataAnnotations.Schema;

namespace TechTrack.AuthService.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id {  get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;

        public DateTime ExpiredAt { get; set; }
        public UserCredentials UserCredentials { get; set; } = null!;
    }
}
