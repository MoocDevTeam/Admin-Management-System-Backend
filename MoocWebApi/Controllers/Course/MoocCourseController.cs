using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Contracts.Course;
using Mooc.Application.Contracts.Course.Dto;
namespace MoocWebApi.Controllers.Course
{
    /// <summary>
    /// Provides endpoints for managing courses.
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class MoocCourseController : ControllerBase
    {

        /// <summary>
        /// Constructor for MoocCourseController.
        /// </summary>
        private readonly IMoocCourseService _courseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoocCourseController"/> class.
        /// </summary>
        /// <param name="courseService">The course service.</param>
        public MoocCourseController(IMoocCourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="input">The course data to create.</param>
        /// <returns>True if the course was created successfully; otherwise, false.</returns>
        [HttpPost]
        // [Authorize]
        public async Task<bool> Add([FromBody] CreateCourseDto input)
        {
            var courseDto = await _courseService.CreateAsync(input);
            return courseDto.Id > 0;
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="input">The course data to update.</param>
        /// <returns>True if the course was updated successfully; otherwise, false.</returns>
        [HttpPost]
        // [Authorize]
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

        /// <summary>
        /// Deletes a course by ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>True if the course was deleted successfully; otherwise, false.</returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _courseService.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Gets a course by its name.
        /// </summary>
        /// <param name="courseName">The name of the course to get.</param>
        /// <returns>The course with the specified name.</returns>
        [HttpGet("{courseName}")]
        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            var course = await _courseService.GetByCourseNameAsync(courseName);
            return course;
        }

        /// <summary>
        /// Gets a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to get.</param>
        /// <returns>The course with the specified ID.</returns>
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

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        [HttpGet]
        public async Task<List<CourseListDto>> GetAll()
        {
            var courseList = await _courseService.GetAllAsync();
            return courseList;
        }
        //  {
        //     var courses = await _courseService.GetAllAsync();
        //     return courses;
        // }


        /// <summary>
        /// Gets all course instances for a given course ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>A list of all course instances for the specified course.</returns>
        [HttpGet("{courseId}")]
        public async Task<List<CourseInstanceDto>> GetCourseInstancesByCourseIdAsync(long courseId)
        {
            var courseInstances = await _courseService.GetCourseInstancesByCourseIdAsync(courseId);
            return courseInstances;
        }

        /// <summary>
        /// Gets a paged list of courses.
        /// </summary>
        /// <param name="input">The filter and paging parameters.</param>
        /// <returns>A paged list of courses.</returns>
        [HttpGet]
        public async Task<PagedResultDto<CourseDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _courseService.GetListAsync(input);
            return pagedResult;
        }
    }
}
