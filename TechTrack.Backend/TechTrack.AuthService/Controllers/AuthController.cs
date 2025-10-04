using Microsoft.AspNetCore.Mvc;

namespace TechTrack.AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Route("index")]
        public IActionResult Login()
        {
            return Ok("HelloWorld");
        }
    }
}
