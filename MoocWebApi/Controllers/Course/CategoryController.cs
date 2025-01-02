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
    public async Task<ActionResult<CategoryDto>> GetByIdAsync(long id)
    {
        var category = await _categoryService.GetAsync(id);
        return Ok(category);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<CategoryDto>> GetByCategoryNameAsync(string name)
    {
        var category = await _categoryService.GetByCategoryNameAsync(name);
        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryDto>> GetAll()
    {
        var category = await _categoryService.GetAllAsync();
        return Ok(category);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryDto>> GetFilteredCategoriesAsync([FromQuery] FilterPagedResultRequestDto input)
    {
        var category = await _categoryService.GetFilteredCategoriesAsync(input);
        return Ok(category);
    }

    [HttpGet("{parentId}")]
    public async Task<ActionResult<CategoryDto>> GetChildCategoriesAsync(long parentId)
    {
        var category= await _categoryService.GetChildCategoriesAsync(parentId);
        return Ok(category);
    }





}

