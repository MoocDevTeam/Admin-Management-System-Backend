using Mooc.Application.Contracts.Course.Dto.Category;

namespace MoocWebApi.Controllers.Course;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
// [Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<bool> Add([FromBody] CreateCategoryDto input)
    {
        var categoryDto = await _categoryService.CreateAsync(input);
        return categoryDto.Id > 0;
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(long id)
    {
        await _categoryService.DeleteAsync(id);
        return true;
    }

    [HttpPost]
    public async Task<bool> Update([FromRoute] long id, [FromBody]UpdateCategoryDto input)
    {
        await _categoryService.UpdateAsync(id, input);
        return true;
    }

    [HttpGet("{id}")]
    public async Task<CategoryDto> GetByIdAsync(long id)
    {
        var category = await _categoryService.GetAsync(id);
        return category;
    }

    [HttpGet("{name}")]
    public async Task<CategoryDto> GetByCategoryNameAsync(string name)
    {
        var category = await _categoryService.GetByCategoryNameAsync(name);
        return category;
    }

    [HttpGet]
    public async Task<List<CategoryDto>> GetAll()
    {
        var category = await _categoryService.GetAllAsync();
        return category;
    }

    [HttpGet]
    public async Task<List<CategoryDto>> GetFilteredCategoriesAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        var category = await _categoryService.GetFilteredCategoriesAsync(input);
        return category;
    }

    [HttpGet("{parentId}")]
    public async Task<List<CategoryDto>> GetChildCategoriesAsync(long parentId)
    {
        var category= await _categoryService.GetChildCategoriesAsync(parentId);
        return category;
    }





}

