using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Contracts.Course 
{
  public interface ISessionService : ICrudService<ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateSessionDto, UpdateSessionDto>


  /// <summary>
  /// 根据 CourseInstanceId 获取所有的 Session 信息
  /// </summary>
  /// <param name="courseInstanceId">课程实例的 Id</param>
  /// <returns>包含所有 Session 的 DTO 列表</returns>
  {
    Task<IEnumerable<ReadSessionDto>> GetAllSessionsByCourseInstanceId(long courseInstanceId);
  }
}