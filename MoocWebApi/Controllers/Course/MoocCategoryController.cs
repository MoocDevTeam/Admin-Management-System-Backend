using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
