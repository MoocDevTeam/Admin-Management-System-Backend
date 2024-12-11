using Microsoft.AspNetCore.Mvc;

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