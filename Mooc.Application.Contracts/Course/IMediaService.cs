using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Contracts.Course
{
  public interface IMediaService : ICrudService<ReadMediaDto, ReadMediaDto, long, FilterPagedResultRequestDto, CreateMediaDto, UpdateMediaDto>
  {
    /// <summary>
    /// Get all media matching the given name (partial match).
    /// </summary>
    /// <param name="mediaName">The media name to search for.</param>
    /// <returns>A list of <see cref="ReadMediaDto"/> objects.</returns>
    Task<IEnumerable<ReadMediaDto>> GetMediaByName(string meidaName);

    /// <summary>
    /// Get all media information based on SessionId.
    /// </summary>
    /// <param name="sessionId">The Id of the session</param>
    /// <returns>List of DTOs that contain all media related to the specified session</returns>
    Task<IEnumerable<ReadMediaDto>> GetMeidaBySessionId(long seesionId);

  }
}
