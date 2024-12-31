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

        [HttpPost]
        public async Task<bool> Add([FromBody] CreateCourseDto input)
        {
            var courseDto = await _courseService.CreateAsync(input);
            return courseDto.Id > 0;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _courseService.DeleteAsync(id);
            return true;
        }

        [HttpGet("{courseName}")]
        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            var course = await _courseService.GetByCourseNameAsync(courseName);
            return course;
        }

        [HttpGet]
        public async Task<List<CourseDto>> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return courses;
        }
    }
}
