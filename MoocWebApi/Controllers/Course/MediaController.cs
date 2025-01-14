using Microsoft.AspNetCore.Authorization;
using Mooc.Application.Contracts.Course.Dto;

namespace MoocWebApi.Controllers.Course
{
  [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
  [Route("api/[controller]/[action]")]
  [ApiController]
  [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
  public class MediaController : ControllerBase
  {
    private readonly IMediaService _mediaService;

    public MediaController(IMediaService mediaService)
    {
      _mediaService = mediaService;
    }

    /// <summary>
    /// Adds a new media record.
    /// </summary>
    /// <param name="input">The media data to create a new media record.</param>
    /// <returns>Returns true if the media was successfully added; otherwise, false.</returns>
    /// <remarks>URL: POST api/Media</remarks>
    [HttpPost]
    public async Task<bool> Add([FromBody] CreateMediaDto input)
    {
      var newMedia = await _mediaService.CreateAsync(input);
      return newMedia.Id > 0;
    }

    /// <summary>
    /// Updates an existing media record.
    /// </summary>
    /// <param name="input">The media data to update the existing media record.</param>
    /// <returns>Returns true if the media was successfully updated; otherwise, false.</returns>
    /// <remarks>URL: POST api/Media</remarks>
    [HttpPost]
    public async Task<bool> Update([FromBody] UpdateMediaDto input)
    {
      var updatedMedia = await _mediaService.UpdateAsync(input.Id, input);
      return updatedMedia.Id > 0;
    }

    /// <summary>
    /// Gets a paged list of media records.
    /// </summary>
    /// <param name="input">The filter and pagination information for fetching media records.</param>
    /// <returns>Returns a paged list of media records based on the given filters and pagination settings.</returns>
    /// <remarks>URL: GET api/Media</remarks>
    [HttpGet]
    public async Task<PagedResultDto<ReadMediaDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
    {
      var pagedResult = await this._mediaService.GetListAsync(input);
      return pagedResult;
    }

    /// <summary>
    /// Gets a single media record by its ID.
    /// </summary>
    /// <param name="id">The ID of the media record to retrieve.</param>
    /// <returns>Returns the media record details for the specified ID.</returns>
    /// <remarks>URL: GET api/Media/{id}</remarks>
    [HttpGet("{id}")]
    public async Task<ReadMediaDto> GetAsync(long id)
    {
      var singleMeidaDetails = await _mediaService.GetAsync(id);
      return singleMeidaDetails;
    }

    /// <summary>
    /// Gets a list of media records based on the specified media name.
    /// </summary>
    /// <param name="mediaName">The name of the media to search for.</param>
    /// <returns>Returns a list of media records that match the given media name.</returns>
    /// <remarks>URL: GET api/Media/{mediaName}</remarks>
    [HttpGet("{mediaName}")]
    public async Task<IEnumerable<ReadMediaDto>> GetMediaByName(string mediaName)
    {
      var mediaRecords = await _mediaService.GetMediaByName(mediaName);
      return mediaRecords;
    }

    /// <summary>
    /// Gets a list of media records related to the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID to filter media records.</param>
    /// <returns>Returns a list of media records associated with the specified session ID.</returns>
    /// <remarks>URL: GET api/Media/{sessionId}</remarks>
    [HttpGet("{sessionId}")]
    public async Task<IEnumerable<ReadMediaDto>> GetMeidaBySessionId(long seesionId)
    {
      var mediaRecords = await _mediaService.GetMeidaBySessionId(seesionId);
      return mediaRecords;
    }

    /// <summary>
    /// Deletes a media record by its ID.
    /// </summary>
    /// <param name="id">The ID of the media record to delete.</param>
    /// <returns>Returns true if the media was successfully deleted; otherwise, false.</returns>
    /// <remarks>URL: DELETE api/Media/{id}</remarks>
    [HttpDelete("{id}")]
    public async Task<bool> Delete(long id)
    {
      await _mediaService.DeleteAsync(id);
      return true;
    }

  }
}
