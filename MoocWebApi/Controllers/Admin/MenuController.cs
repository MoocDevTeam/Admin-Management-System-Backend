using Microsoft.AspNetCore.Mvc;
using Mooc.Core.ExceptionHandling;
using Mooc.Core.WrapperResult;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]
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
            return await _menuService.GetListAsync(input);
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
            //try
            //{
                await _menuService.UpdateAsync(input.Id, input);
            //}
            //catch (EntityNotFoundException)
            //{
            //    HttpContext.Response.StatusCode = 404;
            //    return false;
            //}

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
            //try
            //{
                await _menuService.DeleteAsync(id);
            //}
            //catch(EntityNotFoundException)
            //{
            //    HttpContext.Response.StatusCode = 404;
            //    return false;
            //}
            //catch (MoocValidationException)
            //{
            //    HttpContext.Response.StatusCode = 400;
            //    return false;
            //}
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
            var menu = new MenuDto();
            //try
            //{
              menu =  await _menuService.GetAsync(id);
            //}
            //catch(EntityNotFoundException)
            //{
            //    HttpContext.Response.StatusCode = 404;
            //}
            return menu;
        }

        /// <summary>
        /// Get the menu tree
        /// </summary>
        /// <returns>A list of menus with tree structure</returns>
        [HttpGet]
        public async Task<List<MenuDto>> GetMenuTree()
        {
            return await _menuService.GetMenuTreeAsync();
        }

    }
}
