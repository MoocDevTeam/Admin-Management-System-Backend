using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        // Inject the file upload service
        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        // Endpoint to upload a file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string folderName)
        {
            try
            {
                if (file == null)
                    return BadRequest("No file uploaded");

                // Call the service to upload the file
                var fileUrl = await _fileUploadService.UploadFileAsync(file, folderName);

                // Return the URL of the uploaded file
                return Ok(new { FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
