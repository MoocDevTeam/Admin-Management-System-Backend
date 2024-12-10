using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class MoocTeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
