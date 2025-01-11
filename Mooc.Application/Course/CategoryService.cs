using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto.Category;

namespace Mooc.Application.Course;

public class CategoryService : CrudService<Category, CategoryDto, CategoryDto, long, FilterPagedResultRequestDto, CreateCategoryDto, UpdateCategoryDto>,
    ICategoryService, ITransientDependency

{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CategoryService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
        this._webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var category = await base.GetQueryable().ToListAsync();
        if (category.Count == 0)
            return new List<CategoryDto>();
        return MapToGetListOutputDtos(category);

    }
    public async Task<CategoryDto> GetByCategoryNameAsync(string categoryName)
    {
        var category = await base.GetDbSet().FirstOrDefaultAsync(x => x.CategoryName == categoryName);
        if (category == null)
            return new CategoryDto();
        return MapToGetOutputDto(category);
    }

    public async Task<List<CategoryDto>> GetChildCategoriesAsync(long parentId)
    {
        var category = await base.GetDbSet()
            .Where(c => c.ParentId == parentId)
            .ToListAsync();
        if (category.Count == 0)
            return new List<CategoryDto>();
        return MapToGetListOutputDtos(category);
    }

    public async Task<List<CategoryDto>> GetFilteredCategoriesAsync(FilterPagedResultRequestDto input)
    {
        var query=CreateFilteredQuery(input);
        if(!string.IsNullOrEmpty(input.Filter))
        {
            query = query.Where(x =>
            x.CategoryName.Contains(input.Filter) || x.Id.ToString() == input.Filter);
        }
        var category=await query.ToListAsync();
        return MapToGetListOutputDtos(category);

    }

    protected virtual async Task ValidateCategoryNameAsync(string categoryName)
    {
        var category = await base.GetQueryable().FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        if (category != null)
        {
            throw new EntityAlreadyExistsException($"{categoryName} already exists");
        }
    }

    public override async Task<CategoryDto> CreateAsync(CreateCategoryDto input)
    {
        await ValidateCategoryNameAsync (input.CategoryName);
        return await base.CreateAsync(input);
    }

    public override async Task<CategoryDto> UpdateAsync(long id, UpdateCategoryDto input)
    {
        await ValidateCategoryNameAsync(input.CategoryName);
        return await base.UpdateAsync(id,input);
    }


}
