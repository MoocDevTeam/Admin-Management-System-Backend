using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class MoocTeacherCourseInstanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
