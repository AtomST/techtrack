namespace TechTrack.AuthService.Configuration
{
    public class SecurityOptions
    {
        public string JwtAccessTokenKey { get; set; } = "";
        public int JwtAccessTokenDurationInMinutes { get; set; }
        public int JwtRefreshTokenDurationInDays { get; set; }
        public string AuthHttpOnlyCookieName { get; set; } = "";
        public int BCryptWorkFactor { get; set; }
    }
}
