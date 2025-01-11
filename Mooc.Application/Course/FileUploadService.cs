using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;


namespace Mooc.Application.Course
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AwsS3Config _awsConfig;
        private readonly IAmazonS3 _s3Client;

        public FileUploadService(AwsS3Config awsConfig) 
        {
            _awsConfig = awsConfig;
            _s3Client = new AmazonS3Client(_awsConfig.AccessKeyId, _awsConfig.SecretAccessKey, RegionEndpoint.GetBySystemName(_awsConfig.Region));
        }
    
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            // Validate if the file is null or has zero length
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            // Define the allowed file extensions
            var allowedExtensions = new[] {".mp4", ".avi"};
            var extention = Path.GetExtension(file.FileName).ToLower();

            // Check if the file has a valid extension
            if (!allowedExtensions.Contains(extention))
                throw new ArgumentException("Invalid file type");

            //limite the size of the file (Maximum 800MB)
            const int maxFileSize = 800 * 1024 * 1024;
            if (file.Length > maxFileSize)
                throw new ArgumentException("File size exceeds the maximum limit");

            // Regenerate the file name to avoid conflicts and keep it unique
            var fileName = Path.GetRandomFileName()+extention;
            var filePath = Path.Combine(folderName, fileName);

            // Upload the file to S3 using a stream
            using (var stream = file.OpenReadStream()) 
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _awsConfig.BucketName,
                    Key = filePath,
                    InputStream = stream,
                    ContentType = file.ContentType
                };
                var respose = await _s3Client.PutObjectAsync(putRequest);
                if (respose.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error uploading file to S3");
                }
            }
            var fileUrl = $"https://{_awsConfig.BucketName}.s3.amazonaws.com/{filePath}";
            return fileUrl;
        }
    }
}
