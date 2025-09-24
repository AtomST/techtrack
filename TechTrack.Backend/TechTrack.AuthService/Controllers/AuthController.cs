using Microsoft.AspNetCore.Mvc;

namespace TechTrack.AuthService.Controllers
{
    public class AuthController : ControllerBase
    {
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
