using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Contracts.Course;

namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
    // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class MoocCourseController : ControllerBase
    {
        private readonly IMoocCourseService _courseService;


        public MoocCourseController(IMoocCourseService courseService)
        {
            _courseService = courseService;
        }

        // [HttpGet]
        // public async Task<PagedResultDto<CourseDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        // {
        //     var pagedResult = await _courseService.GetListAsync(input);

        //     return pagedResult;
        // }

        // [HttpGet("{courseName}")]
        // public async Task<IActionResult> GetByCourseNameAsync(string courseName)
        // {
        //     var course = await _courseService.GetByCourseNameAsync(courseName);
        //     if (course == null)
        //         return NotFound(new { Message = $"Course with name '{courseName}' not found." });
        //     return Ok(course);
        // }

        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     var course = await _courseService.GetAllAsync();
        //     if (course == null)
        //     {
        //         return NotFound("No courses found.");
        //     }
        //     return Ok(course);
        // }
    }
}
