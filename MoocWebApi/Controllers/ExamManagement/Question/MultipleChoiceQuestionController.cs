using Mooc.Application.Contracts.ExamManagement;

namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class MultipleChoiceQuestionController : ControllerBase
{
    private readonly IMultipleChoiceQuestionService _multipleChoiceQuestionService;

    public MultipleChoiceQuestionController(IMultipleChoiceQuestionService multipleChoiceQuestionService)
    {
        _multipleChoiceQuestionService = multipleChoiceQuestionService;
    }

    /// <summary>
    /// Get All Multiple Choice Questions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<MultipleChoiceQuestionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<MultipleChoiceQuestionDto> { Items = new List<MultipleChoiceQuestionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _multipleChoiceQuestionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Multiple Choice question By MultipleChoiceQuestionId
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single Multiple Choice Question By Id</returns>
    [HttpGet("{id}")]
    public async Task<MultipleChoiceQuestionDto> GetMultipleChoiceQuestionByIdPageAsync(long id)
    {
        return await _multipleChoiceQuestionService.GetAsync(id);
    }

    /// <summary>
    /// Adds a multiple choice question
    /// </summary>
    /// <param name="input">The Multiple Choice Question to create a new Multiple Choice Question.</param>
    /// <returns>Returns true if the Multiple Choice Question was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateMultipleChoiceQuestionAsync([FromBody] CreateMultipleChoiceQuestionDto input)
    {
        var multipleChoiceQuestionDto = await _multipleChoiceQuestionService.CreateAsync(input);
        return multipleChoiceQuestionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing Multiple Choice Question based on the provided Multiple Choice Question details.
    /// </summary>
    /// <param name="input">The Multiple Choice Question details with updated information.</param>
    /// <returns>Returns true if the Multiple Choice Question was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateMultipleChoiceQuestionAsync([FromBody] UpdateMultipleChoiceQuestionDto input)
    {
        var multipleChoiceQuestionDto = await _multipleChoiceQuestionService.UpdateAsync(input.Id, input);
        return multipleChoiceQuestionDto.Id > 0;
    }

    /// <summary>
    /// Deletes a Multiple Choice Question by its ID.
    /// </summary>
    /// <param name="id">The ID of the multiple choice question to delete.</param>
    /// <returns>Returns true if the multiple choice question was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteMultipleChoiceQuestionAsync(long id)
    {
        await _multipleChoiceQuestionService.DeleteAsync(id);
        return true;
    }
}
