using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class ExamController : ControllerBase
    {

        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        /// <summary>
        /// Get All Exams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<ExamDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            // Validate input parameters (PageIndex, PageSize)
            if (input.PageIndex <= 0 || input.PageSize <= 0)
            {
                // Return an empty PagedResultDto if input is invalid
                return new PagedResultDto<ExamDto> { Items = new List<ExamDto>(), Total = 0 };
            }
            // Fetch paged result from service layer
            return await _examService.GetListAsync(input);
        }

        /// <summary>
        /// Get Exam By ExamId
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Returns Single Exam By Id</returns>
        [HttpGet("{id}")]
        public async Task<ExamDto> GetExamByIdPageAsync(long id)
        {
            return await _examService.GetAsync(id);
        }

        /// <summary>
        /// Adds a exam
        /// </summary>
        /// <param name="input">The exam details to create a new exam.</param>
        /// <returns>Returns true if the exam was successfully added; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpPost]
        public async Task<bool> CreateExamAsync([FromBody] CreateExamDto input)
        {
            var examDto = await _examService.CreateAsync(input);
            return examDto.Id > 0;
        }

        /// <summary>
        /// Updates an existing exam based on the provided exam details.
        /// </summary>
        /// <param name="input">The exam details with updated information.</param>
        /// <returns>Returns true if the exam was successfully updated; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpPost("{id}")]
        public async Task<bool> UpdateExamAsync([FromBody] UpdateExamDto input)
        {
            var examDto = await _examService.UpdateAsync(input.Id, input);

            return examDto.Id > 0;
        }

        /// <summary>
        /// Deletes an exam by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam to delete.</param>
        /// <returns>Returns true if the exam was successfully deleted; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpDelete]
        public async Task<bool> DeleteExamAsync(long id)
        {
            await _examService.DeleteAsync(id);
            return true;
        }
    }
}
