using Microsoft.AspNetCore.Authorization;

namespace MoocWebApi.Controllers.Course
{
    public class MoocMediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}