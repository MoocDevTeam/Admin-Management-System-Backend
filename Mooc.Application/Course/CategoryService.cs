using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto.Category;
using StackExchange.Redis;

namespace Mooc.Application.Course;

public class CategoryService : CrudService<Category, CategoryDto, CategoryDto, long, FilterPagedResultRequestDto, CreateCategoryDto, UpdateCategoryDto>,
    ICategoryService, ITransientDependency

{

    public CategoryService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
    }

    public async Task<PagedResultDto<CategoryDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
        return await base.GetListAsync(input);
    }

    public async Task<CategoryDto> GetByCategoryNameAsync(string categoryName)
    {
        var category = await base.GetDbSet().FirstOrDefaultAsync(x => x.CategoryName == categoryName);
        if (category == null)
            return new CategoryDto();
        return MapToGetOutputDto(category);
    }

    public async Task<List<CategoryDto>> GetChildrenCategoriesAsync(long id)
    {
        var categories = await base.GetDbSet()
            .Where(c => c.ParentId == id)
            .ToListAsync();

        if (!categories.Any())
            return new List<CategoryDto>();

        var categoryDtos = await Task.WhenAll(categories.Select(async category =>
        {
            var dto = MapToGetOutputDto(category);
            dto.ChildrenCategories = await GetChildrenCategoriesAsync(category.Id);
            return dto;
        })).ConfigureAwait(false);
        return categoryDtos.ToList();
    }


    protected virtual async Task ValidateCategoryNameAsync(string categoryName)
    {
        var exit= await base.GetQueryable().AnyAsync(c => c.CategoryName == categoryName);
        if (exit)
        {
            throw new EntityAlreadyExistsException($"{categoryName} already exists");
        }
    }

    protected virtual async Task ValidateCategoryIdAsync(long id)
    {
        var exit = await base.GetQueryable().AnyAsync(x => x.Id == id);
        if (!exit)
        {
            throw new EntityNotFoundException($"Category with Id {id} is not found");
        }
    }

    public override async Task<CategoryDto> CreateAsync(CreateCategoryDto input)
    {
        await ValidateCategoryNameAsync (input.CategoryName);
        return await base.CreateAsync(input);
    }

    public override async Task<CategoryDto> UpdateAsync(long id, UpdateCategoryDto input)
    {
        await ValidateCategoryIdAsync(input.Id);
        await ValidateCategoryNameAsync(input.CategoryName);
        return await base.UpdateAsync(input.Id,input);
    }


}
