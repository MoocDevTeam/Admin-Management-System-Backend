using Microsoft.AspNetCore.Authorization;
using Mooc.Application.Contracts.Course.Dto;

namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Adds a new session based on the provided session details.
        /// </summary>
        /// <param name="input">The session details to create a new session.</param>
        /// <returns>Returns true if the session was successfully added; otherwise, false.</returns>
        /// <remarks>URL: POST api/Session/Add</remarks>
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateOrUpdateSessionDto input)
        {
            var newSession = await _sessionService.CreateAsync(input);
            return newSession.Id > 0;
        }

        /// <summary>
        /// Updates an existing session based on the provided session details.
        /// </summary>
        /// <param name="input">The session details with updated information.</param>
        /// <returns>Returns true if the session was successfully updated; otherwise, false.</returns>
        /// <remarks>URL: POST api/Session/Update</remarks>
        [HttpPost]
        public async Task<bool> Update([FromBody] CreateOrUpdateSessionDto input)
        {
            var updatedSession = await _sessionService.UpdateAsync(input.Id, input);
            return updatedSession.Id > 0;
        }

        /// <summary>
        /// Get paginated session records
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>URL: GET api/Session/GetByPageAsync</remarks>
        [HttpGet]
        public async Task<PagedResultDto<ReadSessionDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await this._sessionService.GetListAsync(input);
            return pagedResult;
        }

        /// <summary>
        /// Retrieves a session by its ID.
        /// </summary>
        /// <param name="id">The ID of the session to retrieve.</param>
        /// <returns>Returns the details of the session with the specified ID.</returns>
        /// <remarks>URL: GET api/Session/{id}</remarks>
        [HttpGet("{id}")]
        public async Task<ReadSessionDto> GetAsync(long id)
        {
            var singleSessionDetails = await _sessionService.GetAsync(id);
            return singleSessionDetails;
        }

        /// <summary>
        /// Retrieves a session by its title.
        /// </summary>
        /// <param name="sessionTitle">The title of the session to retrieve.</param>
        /// <returns>Returns the details of the session with the specified title.</returns>
        /// <remarks>URL: GET api/Session/{sessionTitle}</remarks>
        [HttpGet("{sessionTitle}")]
        public async Task<ReadSessionDto> GetSessionByTitle(string sessionTitle)
        {
            var singleSessionDetails = await _sessionService.GetSessionByTitle(sessionTitle);
            return singleSessionDetails;
        }

        /// <summary>
        /// Retrieves all sessions related to a specific course instance.
        /// </summary>
        /// <param name="courseInstanceId">The ID of the course instance to retrieve sessions for.</param>
        /// <returns>Returns a list of sessions associated with the specified course instance.</returns>
        /// <remarks>URL: GET api/Session/{courseInstanceId}</remarks>
        [HttpGet("{courseInstanceId}")]
        public async Task<IEnumerable<ReadSessionDto>> GetAllSessionsByCourseInstanceId(long courseInstanceId)
        {
            var sessionsForCourseInstance = await _sessionService.GetAllSessionsByCourseInstanceId(courseInstanceId);
            return sessionsForCourseInstance;
        }

        /// <summary>
        /// Deletes a session by its ID.
        /// </summary>
        /// <param name="id">The ID of the session to delete.</param>
        /// <returns>Returns true if the session was successfully deleted; otherwise, false.</returns>
        /// <remarks>URL: DELETE api/Session/{id}</remarks>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _sessionService.DeleteAsync(id);
            return true;
        }

    }
}
