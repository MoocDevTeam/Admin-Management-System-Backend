namespace Mooc.Application.Contracts.Course
{
    public interface ICourseInstanceService : ICrudService<CourseInstanceDto, CourseInstanceDto, long, FilterPagedResultRequestDto, CreateCourseInstanceDto, UpdateCourseInstanceDto>
    {
        Task<List<CourseInstanceDto>> GetByMoocCourseTtileAsync(string moocCourseTtile);
    }
}
