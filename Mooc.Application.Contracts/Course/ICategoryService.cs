namespace Mooc.Application.Contracts.Course;

public interface ICategoryService:ICrudService<CategoryDto,CategoryDto,long, FilterPagedResultRequestDto, CreateEnrollmentDto, UpdateEnrollmentDto>
{
    Task<CategoryDto> GetByCourseNameAsync(string categoryName);
    Task<List<CategoryDto>> GetAllAsync();
};
