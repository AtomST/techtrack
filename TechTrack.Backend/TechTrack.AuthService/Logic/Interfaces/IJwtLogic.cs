namespace TechTrack.AuthService.Logic.Interfaces
{
    public interface IJwtLogic
    {
        public Task<string> GenerateAccessToken(Guid userId, bool isLogin = true);
        public string GenerateRefreshToken();
    }
}
