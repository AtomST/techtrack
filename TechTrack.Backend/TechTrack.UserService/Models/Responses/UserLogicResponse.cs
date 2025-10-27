using Microsoft.AspNetCore.SignalR;

namespace TechTrack.UserService.Models.Responses
{
    public record UserLogicResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiredAt, string UserId);
}
