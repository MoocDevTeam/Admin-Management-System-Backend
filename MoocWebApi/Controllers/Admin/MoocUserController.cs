namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.MoocService))]
    // [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    public class MoocUserController : ControllerBase
    {
        private readonly IMoocUserService _moocUserService;

        public MoocUserController(IMoocUserService moocUserService)
        {
            this._moocUserService = moocUserService;
        }

        ///<summary>
        ///get information for role, username, email and avatar
        /// </summary>
        /// <returns></return>
        [HttpGet]
        public async Task<UserDto> GetMoocUserByUserName(string userName)
        {
            return await _moocUserService.GetByUserNameAsync(userName);
        }
    }
}