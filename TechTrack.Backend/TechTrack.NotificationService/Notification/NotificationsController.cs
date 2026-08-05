using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechTrack.NotificationService.Notification.Logic.Interfaces;
using TechTrack.Shared.Auth;
using TechTrack.Shared.Responses;

namespace TechTrack.NotificationService.Notification
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController(INotificationLogic notificationLogic) : ControllerBase
    {
        [HttpGet("my")]
        [Authorize(Policy = Policies.EmployeeAccess)]
        public async Task<IActionResult> GetUserNotifications()
        {
            var response = await notificationLogic.GetAllUserNotifications(User.GetPrincipalInfo().UserId);
            return Ok(new SuccessResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = response
            });
        }
    }
}
