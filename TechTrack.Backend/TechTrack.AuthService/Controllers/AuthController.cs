using Microsoft.AspNetCore.Mvc;

namespace TechTrack.AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
