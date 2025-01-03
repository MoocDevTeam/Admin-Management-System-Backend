namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Get menus by page
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<MenuDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _menuService.GetListAsync(input);
            return pagedResult;
        }

        /// <summary>
        /// Add a new menu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateMenuDto input)
        {
            var menuDto = await _menuService.CreateAsync(input);
            return menuDto.Id > 0;
        }

        /// <summary>
        /// Update a menu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Update([FromBody] UpdateMenuDto input)
        {
            await _menuService.UpdateAsync(input.Id, input);
            return true;
        }

        /// <summary>
        /// Delete a menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _menuService.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Get a menu by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<MenuDto> GetById(long id)
        {
            return await _menuService.GetAsync(id);
        }

    }
}
