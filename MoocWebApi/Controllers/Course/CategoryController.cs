using Mooc.Application.Contracts.Course.Dto;

namespace MoocWebApi.Controllers.Course;

[ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
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

    /// <summary>
    /// Add Category
    /// </summary>
    /// <param name="input">Details of the new Category.</param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/Add</remarks>

    [HttpPost]
    public async Task<CategoryDto> Add([FromBody] CreateCategoryDto input)
    {
        var categoryDto = await _categoryService.CreateAsync(input);
        return categoryDto;
    }

    /// <summary>
    /// Delete Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/Delete/{id}</remarks>
    [HttpDelete("{id}")]
    public async Task<bool> Delete(long id)
    {
        await _categoryService.DeleteAsync(id);
        return true;
    }


    /// <summary>
    /// Update Category
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/Update</remarks>

    [HttpPost]
    public async Task<bool> Update([FromBody] UpdateCategoryDto input)
    {
        await _categoryService.UpdateAsync(input.Id, input);
        return true;
    }

    /// <summary>
    /// Get Category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetById/{id}</remarks>

    [HttpGet("{id}")]
    public async Task<CategoryDto> GetById(long id)
    {
        var category = await _categoryService.GetAsync(id);
        return category;
    }

    /// <summary>
    /// Get Category by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetById/Name</remarks>

    [HttpGet("{name}")]
    public async Task<CategoryDto> GetCategoryByName(string name)
    {
        var category = await _categoryService.GetByCategoryNameAsync(name);
        return category;
    }

    /// <summary>
    /// Get Category List by filter
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetList</remarks>

    [HttpGet]
    public async Task<PagedResultDto<CategoryDto>> GetList(FilterPagedResultRequestDto input)
    {
        var category = await _categoryService.GetListAsync(input);
        return category;
    }

    /// <summary>
    /// Get Main Category List 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetMainCategories</remarks>

    [HttpGet]
    public async Task<List<CategoryDto>> GetMainCategories()
    {
        var category = await _categoryService.GetAllMainCategoriesAsync();
        return category;
    }

    /// <summary>
    /// Get Category Children List
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetChildrenCategories/{id}</remarks>
    /// 
    [HttpGet]
    public async Task<List<CategoryDto>> GetChildrenCategories(long id)
    {
        var category = await _categoryService.GetChildrenCategoriesAsync(id);
        return category;
    }


    /// <summary>
    /// Get Category Tree 
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// <remarks>URL: POST api/Category/GetAllCategories</remarks>
    /// 
    [HttpGet]
    public async Task<List<CategoryDto>> GetAllCategories()
    {
        var category = await _categoryService.GetAllCategoriesAsync();
        return category;
    }





}

