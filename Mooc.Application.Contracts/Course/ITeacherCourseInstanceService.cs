namespace Mooc.Application.Contracts.Course
{
    public interface ITeacherCourseInstanceService : ICrudService<TeacherCourseInstanceReadDto, TeacherCourseInstanceReadDto, long, FilterPagedResultRequestDto, TeacherCourseInstanceCreateOrUpdateDto, TeacherCourseInstanceCreateOrUpdateDto>
    {
        Task<TeacherCourseInstanceReadDto> GetTeacherCourseInstanceById(long id);
        Task<List<CourseInstanceDto>> GetCourseInstanceListAsync(long id);
        Task<List<TeacherCourseInstanceReadDto>> GetTeacherCourseInstanceByTeacherId(long id);
    }
}
