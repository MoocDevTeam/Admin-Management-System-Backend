using Mooc.Application.Contracts.Course.Dto;

namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<PagedResultDto<TeacherReadDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _teacherService.GetListAsync(input);
            //if (pagedResult.Items.Count > 0)
            //{
            //    foreach (var item in pagedResult.Items)
            //    {
            //        item.CreatedByUser = "";
            //        item.UpdatedByUser = "";
            //    }
            //}
            return pagedResult;
        }

        [HttpPost]
        public async Task<bool> Add([FromBody] CreateOrUpdateTeacherDto input)
        {
            var teacherDto = await _teacherService.CreateAsync(input);
            return teacherDto.Id > 0;
        }


    }
}
