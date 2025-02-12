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

        [HttpPost("{userId}")]
        public async Task<IActionResult> UploadAvatar(string userId, IFormFile file)
        {
            var avatarUrl = await _avatarService.UploadAvatarAsync(userId, file);
            return Ok (new { avatarUrl });
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAvatar(string userId)
        {
            await _avatarService.DeleteAvatarAsync(userId);
            return Ok("Avatar deleted successfully.");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAvatar(string userId)
        {
            var avatarUrl = await _avatarService.GetAvatarUrlAsync(userId);
            return Ok(new { avatarUrl });
        }
    }
}


