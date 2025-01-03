namespace Mooc.Application.Contracts.Course
{
    public interface ICourseInstanceService : ICrudService<CourseInstanceDto, CourseInstanceDto, long, FilterPagedResultRequestDto, CreateCourseInstanceDto, UpdateCourseInstanceDto>
    {
        Task<CourseInstanceDto> GetByMoocCourseTtile(string moocCourseTtile);
    }
}
