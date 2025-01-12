using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get roles by page
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<RoleDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _roleService.GetListAsync(input);
            return pagedResult;
        }

        /// <summary>
        /// Add a new role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateRoleDto input)
        {
            var roleDto = await _roleService.CreateAsync(input);
            return roleDto.Id > 0;
        }

        /// <summary>
        /// Update a role
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Update( [FromBody] UpdateRoleDto input)
        {
            await _roleService.UpdateAsync(input.Id, input);
            return true;
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _roleService.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Get a role by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<RoleDto> GetById(long id)
        {
            return await _roleService.GetAsync(id);
        }


    }
}
