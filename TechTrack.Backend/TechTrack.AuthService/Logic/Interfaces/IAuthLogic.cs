using TechTrack.AuthService.Logic.Models;

namespace TechTrack.AuthService.Logic.Interfaces
{
    public interface IAuthLogic
    {
        public Task<AuthServiceResponse> Login(LoginDto loginDto);
        public void Logout(string refreshToken);
    }
}
