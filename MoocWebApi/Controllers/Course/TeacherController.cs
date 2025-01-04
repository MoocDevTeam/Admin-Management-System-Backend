using Mooc.Application.Contracts.Course.Dto;
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

        [HttpGet("{id}")]
        public async Task<TeacherReadDto> GetAsync(long id)
        {
            var teacherDto = await _teacherService.GetAsync(id);
            return teacherDto;
        }

        [HttpGet("{name}")]
        public async Task<TeacherReadDto> GetTeacherByName(string name)
        {
            var teacherDto = await _teacherService.GetTeacherByName(name);
            return teacherDto;
        }

        [HttpPost]
        public async Task<bool> Add([FromBody] CreateOrUpdateTeacherDto input)
        {
            var teacherDto = await _teacherService.CreateAsync(input);
            return teacherDto.Id > 0;
        }

        [HttpPost]
        public async Task<bool> Update([FromBody] CreateOrUpdateTeacherDto input)
        {
            var teacherDto = await _teacherService.UpdateAsync(input.Id, input);
            return teacherDto.Id > 0;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _teacherService.DeleteAsync(id);
            return true;
        }


    }
}
