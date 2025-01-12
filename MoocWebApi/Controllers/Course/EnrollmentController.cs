namespace MoocWebApi.Controllers.Course;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
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
    /// <summary>
    /// Get Enrollment by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Enrollment/GetById/{id}</remarks>
  
    [HttpGet("{id}")]
    public async Task<EnrollmentDto> GetbyIdAsync(long id)
    {            
        var enrollment = await _enrollmentService.GetAsync(id);    
        return enrollment;
    }

    /// <summary>
    /// Get List Enrollment 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Enrollment/GetList/input</remarks>
    /// 
    [HttpGet]
    public async Task<PagedResultDto<EnrollmentDto>> GetList(FilterPagedResultRequestDto input)
    {
        var enrollment = await _enrollmentService.GetListAsync(input);
        return enrollment ;
    }

    /// <summary>
    /// Add Enrollment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Enrollment/Add/input</remarks>

    [HttpPost]
    public async Task<bool> Add([FromBody] CreateEnrollmentDto input)
    {
        var enrollmentDto = await _enrollmentService.CreateAsync(input);
        return enrollmentDto.Id > 0;
    }

    /// <summary>
    /// Update Enrollment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Enrollment/Update/input</remarks>
    [HttpPost]
    public async Task<bool> Update([FromBody] UpdateEnrollmentDto input)
    {
        await _enrollmentService.UpdateAsync(input.Id, input);
        return true;
    }

    /// <summary>
    /// Delete Enrollment
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Enrollment/delete</remarks>
    [HttpDelete("{id}")]
    public async Task<bool> Delete(long id)
    {
        await _enrollmentService.DeleteAsync(id);
        return true;
    }
}

