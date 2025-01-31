using Mooc.Application.Contracts.Course;

namespace Mooc.Application.Contracts.Course;

public interface ICategoryService:ICrudService<CategoryDto,CategoryDto,long, FilterPagedResultRequestDto, CreateCategoryDto, UpdateCategoryDto>
{
    Task<List<CategoryDto>> GetAllMainCategoriesAsync();
    Task<CategoryDto> GetByCategoryNameAsync(string categoryName);
    Task<List<CategoryDto>> GetChildrenCategoriesAsync(long parentId);
    Task<List<CategoryDto>> GetAllCategoriesAsync();
};
