using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Contracts.Course;
namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class MoocCourseController : ControllerBase
    {

        /// Constructor for MoocCourseController.
        private readonly IMoocCourseService _courseService;

        public MoocCourseController(IMoocCourseService courseService)
        {
            _courseService = courseService;
        }

        /// Adds a new course.
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateCourseDto input)
        {
            var courseDto = await _courseService.CreateAsync(input);
            return courseDto.Id > 0;
        }

        /// Updates an existing course.
        [HttpPost]
        public async Task<bool> Update([FromBody] UpdateCourseDto input)
        {
            var course = await _courseService.GetAsync(input.Id);
            if (course == null)
            {
                HttpContext.Response.StatusCode = 404;
                return false;
            }
            await _courseService.UpdateAsync(input.Id, input);
            return true;
        }

        /// Deletes a course by ID.
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _courseService.DeleteAsync(id);
            return true;
        }

        /// Gets a course by its name.
        [HttpGet("{courseName}")]
        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            var course = await _courseService.GetByCourseNameAsync(courseName);
            return course;
        }

        /// Gets a course by its ID.
        [HttpGet("{id}")]
        public async Task<CourseDto> GetById(long id)
        {
            var course = await _courseService.GetAsync(id);
            if (course == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }
            return course;
        }

        /// Gets all courses.
        [HttpGet]
        public async Task<List<CourseDto>> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return courses;
        }

        /// Gets all course instances for a given course ID.
        [HttpGet("{courseId}")]
        public async Task<List<CourseInstanceDto>> GetCourseInstancesByCourseIdAsync(long courseId)
        {
            var courseInstances = await _courseService.GetCourseInstancesByCourseIdAsync(courseId);
            return courseInstances;
        }

        /// Gets a paged list of courses.
        [HttpGet]
        public async Task<PagedResultDto<CourseDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _courseService.GetListAsync(input);
            return pagedResult;
        }
    }
}
