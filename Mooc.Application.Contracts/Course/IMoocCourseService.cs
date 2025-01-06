namespace Mooc.Application.Contracts.Course
{
    public interface IMoocCourseService : ICrudService<CourseDto, CourseDto, long, FilterPagedResultRequestDto, CreateCourseDto, UpdateCourseDto>
    {
        Task<CourseDto> GetByCourseNameAsync(string courseName);
        Task<List<CourseDto>> GetAllAsync();
        Task<bool> CourseExist(string Title);
        Task<List<CourseInstanceDto>> GetCourseInstancesByCourseIdAsync(long courseId);
        Task<PagedResultDto<CourseDto>> GetListAsync(FilterPagedResultRequestDto input);

    }
}