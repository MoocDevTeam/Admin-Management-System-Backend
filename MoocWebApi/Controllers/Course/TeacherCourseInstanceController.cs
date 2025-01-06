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

        //Get all TeacherCourseInstance
        [HttpGet]
        public async Task<PagedResultDto<TeacherCourseInstanceReadDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _teacherCourseInstanceservice.GetListAsync(input);
            return pagedResult;
        }

        //Get all course instance under  a specific teacher
        [HttpGet]
        public async Task<List<CourseInstanceDto>> GetAllCourseInstanceByTeacherId(long id)
        {
            var courseInstances = await _teacherCourseInstanceservice.GetCourseInstanceListAsync(id);
            return courseInstances;
        }

        //Update TeacherCourseInstance. This is used to modify teacher's permission to edit sessions
        [HttpPost]
        public async Task<bool> Update([FromBody] TeacherCourseInstanceCreateOrUpdateDto input)
        {
            var teacherCourseInstanceReadDto = await _teacherCourseInstanceservice.UpdateAsync(input.Id, input);
            return teacherCourseInstanceReadDto.Id > 0;
        }

        //Assign a teacher to a course instance
        [HttpPost]
        public async Task<bool> AssignTeacherToCourseInstance([FromBody] TeacherCourseInstanceCreateOrUpdateDto input)
        {
            var teacherCourseInstanceDto = await _teacherCourseInstanceservice.CreateAsync(input);
            return teacherCourseInstanceDto.Id > 0;
        }

    }
}
