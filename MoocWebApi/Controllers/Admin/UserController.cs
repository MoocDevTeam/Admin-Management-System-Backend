﻿// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using Microsoft.AspNetCore.Authorization;
using Mooc.Model.Entity;

namespace MoocWebApi.Controllers.Admin
{

    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.BaseService))]
   // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
       
        public UserController(IUserService userService)
        {
            _userService = userService;
 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<UserDto>> GetByPageAsync([FromQuery] FilterPagedResultRequestDto input)
        {
            var pagedResult = await _userService.GetListAsync(input);
            if (pagedResult.Items.Count > 0)
            {
                foreach (var item in pagedResult.Items)
                {
                    item.Password = "";
                }
            }

            return pagedResult;
        }

        /// <summary>
        /// add user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(PermissionConsts.User.Add)]
        [HttpPost]
        public async Task<bool> Add([FromBody] CreateUserDto input)
        {
            var userDto = await _userService.CreateAsync(input);
            return userDto.Id > 0;
        }

        /// <summary>
        /// udpate user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
       // [Authorize(PermissionConsts.User.Update)]
        [HttpPost]
        public async Task<bool> Update([FromBody] UpdateUserDto input)
        {
            await _userService.UpdateAsync(input.Id, input);
            return true;
        }

        /// <summary>
        /// delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       // [Authorize(PermissionConsts.User.Delete)]
        [HttpDelete("{id}")]
        public async Task<bool> Delete(long id)
        {
            await _userService.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        [HttpGet("{userName}")]
        public async Task<UserDto> Get(string userName)
        {
            var user = await _userService.GetByUserNameAsync(userName);
            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return user;
        }

    }
}
