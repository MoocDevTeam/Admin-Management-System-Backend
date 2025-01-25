using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Mooc.Shared.Enum;
using Mooc.Application.Course;
using Mooc.Application.Contracts.Course.Dto;
using Microsoft.AspNetCore.Http;
using Mooc.Core.Utils;


namespace Mooc.Application.Course
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AwsS3Config _awsConfig;
        private readonly IAmazonS3 _s3Client;
        private readonly ISessionService _sessionService;

        public FileUploadService(AwsS3Config awsConfig, ISessionService sessionService)
        {
            _awsConfig = awsConfig;
            _s3Client = new AmazonS3Client(_awsConfig.AccessKeyId, _awsConfig.SecretAccessKey, RegionEndpoint.GetBySystemName(_awsConfig.Region));
            _sessionService = sessionService;
        }

        public async Task<string> UploadLargeFileAsync(IFormFile file, string folderName, long sessionId, int partSizeMb = 5, IProgress<int> progress = null)
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
                var fileUrl = $"https://{_awsConfig.BucketName}.s3.amazonaws.com/{filePath}";

                var createMediaDto  = new CreateMediaDto
                {
                    Id = SnowflakeIdGeneratorUtil.NextId(),
                    FilePath = fileUrl,
                    FileName = fileName,
                    FileType = MediaFileType.Video,
                    SessionId = 1,
                    ThumbnailPath = "/thumbnails/sessions/1/Introduction_to_.Net_video1.png",
                    CreatedByUserId = 1,
                    UpdatedByUserId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now.AddMinutes(1),
                    ApprovalStatus = MediaApprovalStatus.Approved,
                };
                await _sessionService.SaveFileUploadInfoAsync(createMediaDto);

                return fileUrl;
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
