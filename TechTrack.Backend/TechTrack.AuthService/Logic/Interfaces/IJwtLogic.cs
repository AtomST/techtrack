namespace TechTrack.AuthService.Logic.Interfaces
{
    public interface IJwtLogic
    {
        public Task<string> GenerateAccessToken(Guid userId);
        public string GenerateRefreshToken();
    }
}
