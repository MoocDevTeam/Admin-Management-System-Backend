using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class JudgementQuestionController : ControllerBase
{

    private readonly IJudgementQuestionService _judgementQuestionService;

    public JudgementQuestionController(IJudgementQuestionService judgementQuestionService)
    {
        _judgementQuestionService = judgementQuestionService;
    }

    /// <summary>
    /// Get All JudgementQuestion
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<JudgementQuestionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<JudgementQuestionDto> { Items = new List<JudgementQuestionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _judgementQuestionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Exam By JudgementQuestionId
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single JudgementQuestion By Id</returns>
    [HttpGet("{id}")]
    public async Task<JudgementQuestionDto> GetJudgementQuestionByIdPageAsync(long id)
    {
        return await _judgementQuestionService.GetAsync(id);
    }

    /// <summary>
    /// Adds a JudgementQuestion
    /// </summary>
    /// <param name="input">The JudgementQuestion to create a new JudgementQuestion.</param>
    /// <returns>Returns true if the JudgementQuestion was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateJudgementQuestionAsync([FromBody] CreateJudgementQuestionDto input)
    {
        var judgementQuestionDto = await _judgementQuestionService.CreateAsync(input);
        return judgementQuestionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing JudgementQuestion based on the provided JudgementQuestion details.
    /// </summary>
    /// <param name="input">The JudgementQuestion details with updated information.</param>
    /// <returns>Returns true if the JudgementQuestion was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateJudgementQuestionAsync([FromBody] UpdateJudgementQuestionDto input)
    {
        var judgementQuestionDto = await _judgementQuestionService.UpdateAsync(input.Id, input);

        return judgementQuestionDto.Id > 0;
    }

    /// <summary>
    /// Deletes an JudgementQuestion by its ID.
    /// </summary>
    /// <param name="id">The ID of the JudgementQuestion to delete.</param>
    /// <returns>Returns true if the JudgementQuestion was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteJudgementQuestionAsync(long id)
    {
        await _judgementQuestionService.DeleteAsync(id);
        return true;
    }
}