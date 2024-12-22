using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
