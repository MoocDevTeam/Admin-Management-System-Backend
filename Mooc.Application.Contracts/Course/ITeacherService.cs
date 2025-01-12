namespace Mooc.Application.Contracts.Course
{
    public interface ITeacherService : ICrudService<TeacherReadDto, TeacherReadDto, long, FilterPagedResultRequestDto, CreateOrUpdateTeacherDto, CreateOrUpdateTeacherDto>

    {
        Task<TeacherReadDto> GetTeacherByName(string teacher);
    }
}
