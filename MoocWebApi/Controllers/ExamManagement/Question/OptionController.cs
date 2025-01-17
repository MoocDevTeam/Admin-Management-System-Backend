using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
[Route("api/[controller]/[action]")]
[ApiController]
[RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
public class OptionController : ControllerBase
{

    private readonly IOptionService _optionService;

    public OptionController(IOptionService optionService)
    {
        _optionService = optionService;
    }

    /// <summary>
    /// Get All Options
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<OptionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        // Validate input parameters (PageIndex, PageSize)
        if (input.PageIndex <= 0 || input.PageSize <= 0)
        {
            // Return an empty PagedResultDto if input is invalid
            return new PagedResultDto<OptionDto> { Items = new List<OptionDto>(), Total = 0 };
        }
        // Fetch paged result from service layer
        return await _optionService.GetListAsync(input);
    }

    /// <summary>
    /// Get Options By OptionsId
    /// </summary>
    /// <param name="Id">Id</param>
    /// <returns>Returns Single Options By Id</returns>
    [HttpGet("{id}")]
    public async Task<OptionDto> GetOptionIdPageAsync(long id)
    {
        return await _optionService.GetAsync(id);
    }

    /// <summary>
    /// Adds an Option
    /// </summary>
    /// <param name="input">The Option to create a new Option.</param>
    /// <returns>Returns true if the Option was successfully added; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost]
    public async Task<bool> CreateOptionAsync([FromBody] CreateOptionDto input)
    {
        var optionDto = await _optionService.CreateAsync(input);
        return optionDto.Id > 0;
    }

    /// <summary>
    /// Updates an existing Option based on the provided Option details.
    /// </summary>
    /// <param name="input">The Option details with updated information.</param>
    /// <returns>Returns true if the Option was successfully updated; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpPost("{id}")]
    public async Task<bool> UpdateOptionAsync([FromBody] UpdateOptionDto input)
    {
        var optionDto = await _optionService.UpdateAsync(input.Id, input);

        return optionDto.Id > 0;
    }

    /// <summary>
    /// Deletes an Option by its ID.
    /// </summary>
    /// <param name="id">The ID of the Option to delete.</param>
    /// <returns>Returns true if the Option was successfully deleted; otherwise, false.</returns>
    /// <remarks></remarks>
    [HttpDelete]
    public async Task<bool> DeleteOptionAsync(long id)
    {
        await _optionService.DeleteAsync(id);
        return true;
    }
}