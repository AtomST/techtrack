namespace TechTrack.AuthService.Logic.Interfaces
{
    public interface IJwtLogic
    {
        public string GenerateAccessToken(Guid userId);
        public string GenerateRefreshToken();
    }
}
