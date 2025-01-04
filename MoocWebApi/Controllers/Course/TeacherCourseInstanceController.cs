using Mooc.Application.Contracts.Course.Dto;

namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class TeacherCourseInstanceController : ControllerBase
    {
        private readonly ITeacherCourseInstanceService _teacherCourseInstanceservice;

        public TeacherCourseInstanceController(ITeacherCourseInstanceService teacherCourseInstance)
        {
            _teacherCourseInstanceservice = teacherCourseInstance;
        }
        [HttpGet]
        public async Task<PagedResultDto<TeacherCourseInstanceReadDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _teacherCourseInstanceservice.GetListAsync(input);
            return pagedResult;
        }

        [HttpPost]
        public async Task<bool> Update([FromBody] TeacherCourseInstanceCreateOrUpdateDto input)
        {
            var teacherCourseInstanceReadDto = await _teacherCourseInstanceservice.UpdateAsync(input.Id, input);
            return teacherCourseInstanceReadDto.Id > 0;
        }


    }
}
