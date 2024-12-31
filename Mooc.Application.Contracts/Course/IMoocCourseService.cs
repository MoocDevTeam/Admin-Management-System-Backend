using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Course
{
    public interface IMoocCourseService : ICrudService<CourseDto, CourseDto, long, FilterPagedResultRequestDto, CreateCourseDto, UpdateCourseDto>
    {
        Task<CourseDto> GetByCourseNameAsync(string courseName);
        Task<List<CourseDto>> GetAllAsync();
        Task<bool> CourseExist(string Title);
    }
}