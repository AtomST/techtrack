namespace TechTrack.AuthService.Logic.Interfaces
{
    interface IJwtLogic
    {
        public string GenerateAccessToken(Guid userId);
        public string GenerateRefreshToken();
    }
}
