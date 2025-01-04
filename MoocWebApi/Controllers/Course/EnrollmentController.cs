namespace MoocWebApi.Controllers.Course;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
// [Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;

    }

    [HttpGet("{id}")]
    public async Task<EnrollmentDto> GetbyIdAsync(long id)
    {            
        var enrollment = await _enrollmentService.GetAsync(id);    
        return enrollment;
    }

    [HttpPost]
    public async Task<bool> Add([FromBody] CreateEnrollmentDto input)
    {
        var enrollmentDto = await _enrollmentService.CreateAsync(input);
        return enrollmentDto.Id > 0;
    }

    [HttpPost]
    public async Task<bool> Update([FromBody] UpdateEnrollmentDto input)
    {
        await _enrollmentService.UpdateAsync(input.Id, input);
        return true;
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(long id)
    {
        await _enrollmentService.DeleteAsync(id);
        return true;
    }
}

