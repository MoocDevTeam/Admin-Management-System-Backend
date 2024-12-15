using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class TeacherCourseInstanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
