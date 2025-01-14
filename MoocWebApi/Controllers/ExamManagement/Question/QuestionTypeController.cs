using Mooc.Application.Contracts.ExamManagement;
using Mooc.Application.Contracts.ExamManagement.Dto.QuestionType;

namespace MoocWebApi.Controllers.ExamManagement
{

    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class QuestionTypeController : ControllerBase
    {
        private readonly IQuestionTypeService _questionTypeService;

        public QuestionTypeController(IQuestionTypeService questionTypeService)
        {
            _questionTypeService = questionTypeService;
        }

        [HttpGet]
        public async Task<PagedResultDto<QuestionTypeDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            return await _questionTypeService.GetListAsync(input);
        }

        [HttpGet("name/{questionTypeName}")]
        public async Task<QuestionTypeDto> GetQuestionTypeByNameAsync(string questionTypeName)
        {
            return await _questionTypeService.GetQuestionTypeByNameAsync(questionTypeName);
        }

        [HttpPost]
        public async Task<QuestionTypeDto> CreatAsync([FromBody] CreateQuestionTypeDto input)
        {
            return await _questionTypeService.CreateAsync(input);
        }

        [HttpPost("{id}")]
        public async Task<QuestionTypeDto> UpdateAsync(long id, [FromBody] UpdateQuestionTypeDto input)
        {
            return await _questionTypeService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public async Task<bool> DeleteAsync(long id)
        {
            await _questionTypeService.DeleteAsync(id);
            return true;
        }
    }
}
