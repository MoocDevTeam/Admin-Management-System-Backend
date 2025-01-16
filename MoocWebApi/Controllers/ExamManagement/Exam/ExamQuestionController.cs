using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class ExamQuestionController
{
    private readonly IExamQuestionService _examQuestionService;

    public ExamQuestionController(IExamQuestionService examQuestionService)
    {
        _examQuestionService = examQuestionService;
    }

    /// <summary>
    /// Get All Exam Questions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<ExamQuestionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<ExamQuestionDto> { Items = new List<ExamQuestionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _examQuestionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Exam By Exam Question Id
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single Exam Question By Id</returns>
    [HttpGet("{id}")]
    public async Task<ExamQuestionDto> GetExamQuestionByIdPageAsync(long id)
    {
        return await _examQuestionService.GetAsync(id);
    }

    /// <summary>
    /// Adds a exam Question
    /// </summary>
    /// <param name="input">The exam question details to create a new exam question.</param>
    /// <returns>Returns true if the exam question was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateExamQuestionAsync([FromBody] CreateExamQuestionDto input)
    {
        var examQuestionDto = await _examQuestionService.CreateAsync(input);
        return examQuestionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing exam Question based on the provided exam question details.
    /// </summary>
    /// <param name="input">The exam question details with updated information.</param>
    /// <returns>Returns true if the exam Question was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateExamQuestionAsync([FromBody] UpdateExamQuestionDto input)
    {
        var examQuestionDto = await _examQuestionService.UpdateAsync(input.Id, input);

        return examQuestionDto.Id > 0;
    }

    /// <summary>
    /// Deletes an exam Question by its ID.
    /// </summary>
    /// <param name="id">The ID of the exam Question to delete.</param>
    /// <returns>Returns true if the Exam Question was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteExamQuestionAsync(long id)
    {
        await _examQuestionService.DeleteAsync(id);
        return true;
    }
}
