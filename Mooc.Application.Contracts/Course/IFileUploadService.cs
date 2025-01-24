using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Course
{
    /// <summary>
    /// Interface for handling file upload functionality.
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// Uploads a large file to the specified folder in multiple parts.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="folderName">The target folder name where the file will be uploaded.</param>
        /// <param name="partSizeMb">The size of each part in megabytes for chunked upload. Default is 5 MB.</param>
        /// <param name="progress">A callback that provides the upload progress in percentage (0-100).</param>
        /// <returns>Returns the URL of the uploaded file.</returns>
        Task<string> UploadLargeFileAsync(IFormFile file, string folderName, int partSizeMb = 5, IProgress<int> progress = null);
    }
}
