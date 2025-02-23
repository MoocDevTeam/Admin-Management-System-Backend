using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Admin;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]
    [RequestFormLimits(MultipartBodyLengthLimit = 52428800)]
    /// <summary>
    /// Controller for managing user avatars, including upload, retrieval, and deletion.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            _avatarService = avatarService;
        }

        [HttpPost("{userName}")]
        public async Task<IActionResult> UploadAvatar(string userName, IFormFile file)
        {
            var avatarUrl = await _avatarService.UploadAvatarAsync(userName, file);
            return Ok (new { avatarUrl });
        }

        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteAvatar(string userName)
        {
            await _avatarService.DeleteAvatarAsync(userName);
            return Ok("Avatar deleted successfully.");
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAvatar(string userName)
        {
            var avatarUrl = await _avatarService.GetAvatarUrlAsync(userName);
            return Ok(new { avatarUrl });
        }
    }
}


