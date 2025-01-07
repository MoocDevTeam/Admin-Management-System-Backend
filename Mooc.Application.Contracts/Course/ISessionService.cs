using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Contracts.Course 
{
  public interface ISessionService : ICrudService<ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateOrUpdateSessionDto, CreateOrUpdateSessionDto>



  {
    /// <summary>
    /// Get all Session information based on CourseInstanceId.
    /// </summary>
    /// <param name="courseInstanceId">The Id of the course instance</param>
    /// <returns>List of DTOs that contain all Sessions</returns>
    Task<IEnumerable<ReadSessionDto>> GetAllSessionsByCourseInstanceId(long courseInstanceId);

    /// <summary>
    /// Get a specific Session by its title.
    /// </summary>
    /// <param name="sessionName">The title of the session</param>
    /// <returns>A DTO of the session with the specified name</returns>
    Task<ReadSessionDto> GetSessionByTitle (string sessionName);
  }
}