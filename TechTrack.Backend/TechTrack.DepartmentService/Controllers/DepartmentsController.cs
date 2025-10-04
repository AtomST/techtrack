using Microsoft.AspNetCore.Mvc;

namespace TechTrack.DepartmentService.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
