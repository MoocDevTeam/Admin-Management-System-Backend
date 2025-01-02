using Mooc.Application.Contracts.Course.Dto.Category;

namespace Mooc.Application.Contracts.Course;

public interface ICategoryService:ICrudService<CategoryDto,CategoryDto,long, FilterPagedResultRequestDto, CreateCategoryDto, UpdateCategoryDto>
{
    Task<CategoryDto> GetByCategoryNameAsync(string categoryName);
    Task<List<CategoryDto>> GetAllAsync();
    Task<List<CategoryDto>> GetChildCategoriesAsync(long parentId);
    Task<List<CategoryDto>> GetFilteredCategoriesAsync(FilterPagedResultRequestDto input);
};
