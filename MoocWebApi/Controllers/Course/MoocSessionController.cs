using Microsoft.AspNetCore.Authorization;

namespace MoocWebApi.Controllers.Course
{
    public class MoocSessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
