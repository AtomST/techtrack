using TechTrack.AuthService.Logic.Models;

namespace TechTrack.AuthService.Logic.Interfaces
{
    public interface IAuthLogic
    {
        public Task<AuthServiceResponse> LoginAsync(LoginDto loginDto);
        public Task<AuthServiceResponse> RefreshAsync(string refreshToken);
        public void Logout(string refreshToken);
    }
}
