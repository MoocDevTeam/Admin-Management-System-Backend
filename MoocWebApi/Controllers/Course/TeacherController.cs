using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Retrieves a paginated list of teachers based on the specified filter criteria.
        /// </summary>
        /// <param name="input">The pagination and filtering criteria.</param>
        /// <returns>A paginated result containing teacher information.</returns>
        [HttpGet]
        public async Task<PagedResultDto<TeacherReadDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            // Validate input parameters (PageIndex, PageSize)
            if (input.PageIndex <= 0 || input.PageSize <= 0)
            {
                // Return an empty PagedResultDto if input is invalid
                return new PagedResultDto<TeacherReadDto> { Items = new List<TeacherReadDto>(), Total = 0 };
            }
            // Fetch paged result from service layer
            var pagedResult = await _teacherService.GetListAsync(input);
            return pagedResult;
        }

        /// <summary>
        /// Retrieves details of a teacher by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher.</param>
        /// <returns>The teacher details.</returns>
        [HttpGet("{id}")]
        public async Task<TeacherReadDto> GetAsync(long id)
        {
            var teacherDto = await _teacherService.GetAsync(id);
            return teacherDto;
        }

        /// <summary>
        /// Retrieves details of a teacher by their name.
        /// </summary>
        /// <param name="name">The name of the teacher.</param>
        /// <returns>The teacher details.</returns>
        [HttpGet("{name}")]
        public async Task<TeacherReadDto> GetTeacherByName(string name)
        {
            var teacherDto = await _teacherService.GetTeacherByName(name);
            return teacherDto;
        }

        /// <summary>
        /// Adds a new teacher to the system.
        /// </summary>
        /// <param name="input">The details of the teacher to be added.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> Add([FromBody] CreateOrUpdateTeacherDto input)
        {
            var teacherDto = await _teacherService.CreateAsync(input);
            return teacherDto.Id > 0;
        }

        /// <summary>
        /// Updates the details of an existing teacher.
        /// </summary>
        /// <param name="input">The updated details of the teacher.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        [HttpPost]
        public async Task<bool> Update([FromBody] CreateOrUpdateTeacherDto input)
        {
            var teacherDto = await _teacherService.UpdateAsync(input.Id, input);
            return teacherDto.Id > 0;
        }

        /// <summary>
        /// Deletes a teacher from the system by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the teacher to be deleted.</param>
        /// <returns>True if the operation is successful, otherwise false.</returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _teacherService.DeleteAsync(id);
            return true;
        }


    }
}
