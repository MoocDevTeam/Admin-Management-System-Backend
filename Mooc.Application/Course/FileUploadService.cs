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

        public async Task<string> UploadLargeFileAsync(IFormFile file, string folderName, int partSizeMb = 5, IProgress<int> progress = null)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            // Validate file extension
            var allowedExtensions = new[] { ".mp4", ".avi" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type");

            // Regenerate the file name
            var fileName = $"{Path.GetRandomFileName()}{extension}";
            var filePath = Path.Combine(folderName, fileName).Replace("\\", "/");

            // Convert part size to bytes
            var partSize = partSizeMb * 1024 * 1024;

            // Initialize multipart upload
            var initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = filePath,
                ContentType = file.ContentType
            };

            var initiateResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
            var uploadId = initiateResponse.UploadId;

            var partETags = new List<PartETag>();
            long totalBytes = file.Length;
            long uploadedBytes = 0;

            try
            {
                // Upload parts
                using (var fileStream = file.OpenReadStream())
                {
                    var partNumber = 1;
                    var buffer = new byte[partSize];
                    int bytesRead;
                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        using (var partStream = new MemoryStream(buffer, 0, bytesRead))
                        {
                            var uploadPartRequest = new UploadPartRequest
                            {
                                BucketName = _awsConfig.BucketName,
                                Key = filePath,
                                UploadId = uploadId,
                                PartNumber = partNumber,
                                PartSize = bytesRead,
                                InputStream = partStream
                            };

                            var uploadPartResponse = await _s3Client.UploadPartAsync(uploadPartRequest);
                            partETags.Add(new PartETag(partNumber, uploadPartResponse.ETag));
                            partNumber++;

                            // Update the uploaded bytes and progress
                            uploadedBytes += bytesRead;
                            var percentage = (int)((uploadedBytes * 100) / totalBytes);
                            progress?.Report(percentage); // Report progress to the front-end
                        }
                    }
                }

                // Complete multipart upload
                var completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = _awsConfig.BucketName,
                    Key = filePath,
                    UploadId = uploadId,
                    PartETags = partETags
                };

                var completeResponse = await _s3Client.CompleteMultipartUploadAsync(completeRequest);
                return $"https://{_awsConfig.BucketName}.s3.amazonaws.com/{filePath}";
            }
            catch (Exception ex)
            {
                // Abort multipart upload on failure
                await _s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
                {
                    BucketName = _awsConfig.BucketName,
                    Key = filePath,
                    UploadId = uploadId
                });

                throw new Exception($"Error uploading file: {ex.Message}", ex);
            }
        }
    }
}
