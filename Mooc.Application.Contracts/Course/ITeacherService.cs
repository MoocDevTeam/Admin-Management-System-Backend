using Mooc.Application.Contracts.Course.Dto;
using Mooc.Model.Entity.Course;

namespace Mooc.Application.Contracts.Course
{
    public interface ITeacherService : ICrudService<TeacherReadDto, TeacherReadDto, long, FilterPagedResultRequestDto, CreateOrUpdateTeacherDto, CreateOrUpdateTeacherDto>

    {
        Task<TeacherReadDto> GetTeacherByName(string teacher);
    }
}
