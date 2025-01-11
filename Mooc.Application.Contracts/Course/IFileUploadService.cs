using Microsoft.AspNetCore.Http;

namespace Mooc.Application.Contracts.Course
{
    /// <summary>
    /// Interface for handling file upload functionality.
    /// </summary>
    public interface IFileUploadService
    {

        /// <summary>
        /// Uploads a file to the specified folder.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="folderName">The target folder name where the file will be uploaded.</param>
        /// <returns>Returns the URL of the uploaded file.</returns>
        Task<string> UploadFileAsync(IFormFile file, string folderName);
    }
}
