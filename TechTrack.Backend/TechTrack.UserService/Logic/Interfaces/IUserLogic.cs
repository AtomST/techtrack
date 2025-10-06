using TechTrack.UserService.Models.Requests;
using TechTrack.UserService.Models.Responses;

namespace TechTrack.UserService.Logic.Interfaces
{
    public interface IUserLogic
    {
        public Task<UserLogicResponse> Register(RegisterDto dto);
    }
}
