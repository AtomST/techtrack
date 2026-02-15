using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechTrack.MaintenanceService.Controllers
{
    [ApiController]
    [Route("api/[controller]/test")]
    public class MaintenanceController : ControllerBase
    {
        // GET: TestController
        public ActionResult Index()
        {
            return Ok("Ok");
        }
    }
}
