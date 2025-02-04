namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseInstanceController : ControllerBase
    {
        private readonly ICourseInstanceService _courseInstanceService;

        public CourseInstanceController(ICourseInstanceService courseInstanceService)
        {
            this._courseInstanceService = courseInstanceService;
        }

        /// <summary>
        /// Get paginated courseInstances
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>URL: GET api/CourseInstance/GetByPageAsync</remarks>
        [HttpGet]
        public async Task<PagedResultDto<CourseInstanceDto>> GetByPageAsync ([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await this._courseInstanceService.GetListAsync(input);
            return pagedResult;
        }

        /// <summary>
        /// Get courseInstances by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>URL: GET api/CourseInstance/GetById/{id}</remarks>
        [HttpGet("{id}")]
        public async Task<CourseInstanceDto> GetById (long id)
        {
            return await this._courseInstanceService.GetAsync(id);
        }

        /// <summary>
        /// Get courseInstances by moocCourse title.
        /// </summary>
        /// <param name="moocCourseTitle">The title of the mooc course.</param>
        /// <remarks>URL: GET api/CourseInstance/GetByMoocCourseTitleAsync/{moocCourseTitle}</remarks>
        //[HttpGet("{moocCourseTitle}")]
        //public async Task<List<CourseInstanceDto>> GetByMoocCourseTitleAsync(string moocCourseTitle)
        //{
        //    return await this._courseInstanceService.GetByMoocCourseTtileAsync(moocCourseTitle);
        //}

        /// <summary>
        /// Add courseInstance
        /// </summary>
        /// <param name="input">Details of the new course instance.</param>
        /// <returns></returns>
        /// <remarks>URL: POST api/CourseInstance/Add</remarks>
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateCourseInstanceDto input)
        {
            var courseInstanceDto = await this._courseInstanceService.CreateAsync(input);
            return courseInstanceDto.Id > 0;
        }

        /// <summary>
        /// udpate courseInstance
        /// </summary>
        /// <param name="input">Updated details of the course instance.</param>
        /// <returns></returns>
        /// <remarks>URL: POST api/CourseInstance/Update</remarks>
        [HttpPost]
        public async Task<bool> Update([FromBody] UpdateCourseInstanceDto input)
        {
            await this._courseInstanceService.UpdateAsync(input.Id, input);
            return true;
        }

        /// <summary>
        /// delete courseInstance
        /// </summary>
        /// <param name="id">The ID of the course instance to delete.</param>
        /// <returns></returns>
        /// <remarks>URL: DELETE api/CourseInstance/Delete/{id}</remarks>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await this._courseInstanceService.DeleteAsync(id);
            return true;
        }
    }
}
