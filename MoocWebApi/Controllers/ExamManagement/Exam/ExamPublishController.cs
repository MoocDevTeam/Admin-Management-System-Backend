using Mooc.Application.Contracts.ExamManagement;


namespace MoocWebApi.Controllers.ExamManagement
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.ExamManagement))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class ExamPublishController : ControllerBase
    {
        private readonly IExamPublishService _examPublishService;

        public ExamPublishController(IExamPublishService examPublishService)
        {
            _examPublishService = examPublishService;
        }

        /// <summary>
        /// Get All Exam Publishes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<ExamPublishDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            // Validate input parameters (PageIndex, PageSize)
            if (input.PageIndex <= 0 || input.PageSize <= 0)
            {
                // Return an empty PagedResultDto if input is invalid
                return new PagedResultDto<ExamPublishDto> { Items = new List<ExamPublishDto>(), Total = 0 };
            }
            // Fetch paged result from service layer
            return await _examPublishService.GetListAsync(input);
        }

        /// <summary>
        /// Get Exam By Exam Publish Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Returns Single Exam Publish By Id</returns>
        [HttpGet("{id}")]
        public async Task<ExamPublishDto> GetExamPublishByIdPageAsync(long id)
        {
            return await _examPublishService.GetAsync(id);
        }

        /// <summary>
        /// Adds a exam Publish
        /// </summary>
        /// <param name="input">The exam Publish details to create a new exam Publish.</param>
        /// <returns>Returns true if the exam question was successfully added; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpPost]
        public async Task<bool> CreateExamPublishAsync([FromBody] CreateExamPublishDto input)
        {
            var examPublishDto = await _examPublishService.CreateAsync(input);
            return examPublishDto.Id > 0;
        }

        /// <summary>
        /// Updates an existing exam Publish based on the provided exam Publish details.
        /// </summary>
        /// <param name="input">The exam Publish details with updated information.</param>
        /// <returns>Returns true if the exam Publish was successfully updated; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpPost("{id}")]
        public async Task<bool> UpdateExamPublishAsync([FromBody] UpdateExamPublishDto input)
        {
            var examPublishDto = await _examPublishService.UpdateAsync(input.Id, input);

            return examPublishDto.Id > 0;
        }

        /// <summary>
        /// Deletes an exam Publish by its ID.
        /// </summary>
        /// <param name="id">The ID of the exam Publish to delete.</param>
        /// <returns>Returns true if the Exam Publish was successfully deleted; otherwise, false.</returns>
        /// <remarks></remarks>
        [HttpDelete]
        public async Task<bool> DeleteExamPublishAsync(long id)
        {
            await _examPublishService.DeleteAsync(id);
            return true;
        }
    }
}
