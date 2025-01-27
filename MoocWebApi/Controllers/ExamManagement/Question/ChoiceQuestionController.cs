using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class ChoiceQuestionController : ControllerBase
{

    private readonly IChoiceQuestionService _choiceQuestionService;

    public ChoiceQuestionController(IChoiceQuestionService choiceQuestionService)
    {
        _choiceQuestionService = choiceQuestionService;
    }

    /// <summary>
    /// Get All Choice Questions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<ChoiceQuestionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<ChoiceQuestionDto> { Items = new List<ChoiceQuestionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _choiceQuestionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Choice question By ChoiceQuestionId
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single ChoiceQuestion By Id</returns>
    [HttpGet("{id}")]
    public async Task<ChoiceQuestionDto> GetChoiceQuestionByIdPageAsync(long id)
    {
        return await _choiceQuestionService.GetAsync(id);
    }

    /// <summary>
    /// Adds a choice question
    /// </summary>
    /// <param name="input">The ChoiceQuestion to create a new ChoiceQuestion.</param>
    /// <returns>Returns true if the ChoiceQuestion was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateChoiceQuestionAsync([FromBody] CreateChoiceQuestionDto input)
    {
        var choiceQuestionDto = await _choiceQuestionService.CreateAsync(input);
        return choiceQuestionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing ChoiceQuestion based on the provided ChoiceQuestion details.
    /// </summary>
    /// <param name="input">The ChoiceQuestion details with updated information.</param>
    /// <returns>Returns true if the ChoiceQuestion was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateChoiceQuestionAsync([FromBody] UpdateChoiceQuestionDto input)
    {
        var choiceQuestionDto = await _choiceQuestionService.UpdateAsync(input.Id, input);

        return choiceQuestionDto.Id > 0;
    }

    /// <summary>
    /// Deletes an ChoiceQuestion by its ID.
    /// </summary>
    /// <param name="id">The ID of the choice question to delete.</param>
    /// <returns>Returns true if the choice question was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteChoiceQuestionAsync(long id)
    {
        await _choiceQuestionService.DeleteAsync(id);
        return true;
    }
}