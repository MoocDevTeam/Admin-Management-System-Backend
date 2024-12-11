using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class MoocCourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
