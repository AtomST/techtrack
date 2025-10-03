namespace TechTrack.AuthService.Logic.Models
{
    public class AuthServiceResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
