using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Contracts.Course;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MoocWebApi.Hubs;

namespace MoocWebApi.Controllers.Course
{
    /// <summary>
    /// Controller for handling file uploads.
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IHubContext<FileUploadHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadController"/> class.
        /// </summary>
        /// <param name="fileUploadService">The file upload service.</param>
        /// <param name="hubContext">The SignalR hub context for broadcasting progress updates.</param>
        public FileUploadController(IFileUploadService fileUploadService, IHubContext<FileUploadHub> hubContext)
        {
            _fileUploadService = fileUploadService;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Uploads a large file in chunks with progress reporting.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="folderName">The folder where the file will be saved.</param>
        /// <param name="partSizeMb">The size of each part in MB for chunked uploads (default is 5MB).</param>
        /// <returns>The URL of the uploaded file.</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string folderName, [FromForm] int partSizeMb = 5)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                // Create progress report callback
                var progress = new Progress<int>(async percentage =>
                {
                    // Send progress update to clients via SignalR
                    await _hubContext.Clients.All.SendAsync("ReceiveProgressUpdate", percentage);
                    Console.WriteLine($"Upload Progress: {percentage}%");
                });

                // Call the service to upload the file
                var fileUrl = await _fileUploadService.UploadLargeFileAsync(file, folderName, partSizeMb, progress);

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
