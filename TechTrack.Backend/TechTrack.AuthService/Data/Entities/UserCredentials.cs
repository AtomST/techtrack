using System.ComponentModel.DataAnnotations;

namespace TechTrack.AuthService.Data.Entities
{
    public class UserCredentials
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public UserInfoCache? UserInfoCache { get; set; }
        
    }
}
