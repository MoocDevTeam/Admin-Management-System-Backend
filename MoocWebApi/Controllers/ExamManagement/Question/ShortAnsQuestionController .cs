using Mooc.Application.Contracts.ExamManagement;
using Mooc.Application.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class ShortAnsQuestionController : ControllerBase
{

    private readonly IShortAnsQuestionService _shortAnsQuestionService;

    public ShortAnsQuestionController(IShortAnsQuestionService shortAnsQuestionService)
    {
        _shortAnsQuestionService = shortAnsQuestionService;
    }

    /// <summary>
    /// Get All Choice Questions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<ShortAnsQuestionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<ShortAnsQuestionDto> { Items = new List<ShortAnsQuestionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _shortAnsQuestionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Exam By ShortAnsQuestionId
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single ShortAnsQuestion By Id</returns>
    [HttpGet("{id}")]
    public async Task<ShortAnsQuestionDto> GetShortAnsQuestionByIdPageAsync(long id)
    {
        return await _shortAnsQuestionService.GetAsync(id);
    }

    /// <summary>
    /// Adds a ShortAnsQuestion
    /// </summary>
    /// <param name="input">The ChoiceQuestion to create a new ChoiceQuestion.</param>
    /// <returns>Returns true if the ChoiceQuestion was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateShortAnsQuestionAsync([FromBody] CreateShortAnsQuestionDto input)
    {
        var shortAnsQuestionDto = await _shortAnsQuestionService.CreateAsync(input);
        return shortAnsQuestionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing ChoiceQuestion based on the provided ChoiceQuestion details.
    /// </summary>
    /// <param name="input">The ChoiceQuestion details with updated information.</param>
    /// <returns>Returns true if the ChoiceQuestion was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateShortAnsQuestionAsync([FromBody] UpdateShortAnsQuestionDto input)
    {
        var shortAnsQuestionDto = await _shortAnsQuestionService.UpdateAsync(input.Id, input);

        return shortAnsQuestionDto.Id > 0;
    }

    /// <summary>
    /// Deletes an ShortAnsQuestion by its ID.
    /// </summary>
    /// <param name="id">The ID of the ShortAnsQuestion to delete.</param>
    /// <returns>Returns true if the ShortAnsQuestion was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteShortAnsQuestionAsync(long id)
    {
        await _shortAnsQuestionService.DeleteAsync(id);
        return true;
    }
}