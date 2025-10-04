using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechTrack.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthClient _client;
        public UsersController(AuthClient client)
        {
            _client = client;
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> Test(string name)
        {
            var response = await _client.SayHelloAsync(name);
            return Ok(response);
        }
    }
}
