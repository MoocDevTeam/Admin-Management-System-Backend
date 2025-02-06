using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Mooc.Shared.Enum;
using Mooc.Application.Course;
using Mooc.Application.Contracts.Course.Dto;
using Microsoft.AspNetCore.Http;
using Mooc.Core.Utils;
using Microsoft.AspNetCore.SignalR;
using Mooc.Shared.Hubs;

namespace Mooc.Application.Course
{

    public class FileUploadService : IFileUploadService
    {
        private readonly AwsS3Config _awsConfig;
        private readonly IAmazonS3 _s3Client;
        private readonly ISessionService _sessionService;
        private readonly IHubContext<FileUploadHub> _hubContext;

        public FileUploadService(
        AwsS3Config awsConfig,
        ISessionService sessionService,
        IHubContext<FileUploadHub> hubContext)

        {
            _awsConfig = awsConfig;
            _s3Client = new AmazonS3Client(_awsConfig.AccessKeyId, _awsConfig.SecretAccessKey, RegionEndpoint.GetBySystemName(_awsConfig.Region));
            _sessionService = sessionService;
            _hubContext = hubContext;
        }

        public async Task<string> UploadLargeFileAsync(IFormFile file, string folderName, long sessionId, string uploadId, int partSizeMb = 5)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded");

            var allowedExtensions = new[] { ".mp4" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type");

            var fileName = $"{Path.GetRandomFileName()}{extension}";
            var filePath = Path.Combine(folderName, fileName).Replace("\\", "/");
            var partSize = partSizeMb * 1024 * 1024;

            var initiateRequest = new InitiateMultipartUploadRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = filePath,
                ContentType = file.ContentType
            };

            var initiateResponse = await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
            var s3UploadId = initiateResponse.UploadId;

            var partETags = new List<PartETag>();
            long totalBytes = file.Length;
            long uploadedBytes = 0;

            try
            {
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
                                UploadId = s3UploadId,
                                PartNumber = partNumber,
                                PartSize = bytesRead,
                                InputStream = partStream
                            };

                            var uploadPartResponse = await _s3Client.UploadPartAsync(uploadPartRequest);
                            partETags.Add(new PartETag(partNumber, uploadPartResponse.ETag));
                            partNumber++;

                            uploadedBytes += bytesRead;
                            var percentage = (int)((uploadedBytes * 100) / totalBytes);

                            await _hubContext.Clients.Group(uploadId).SendAsync("ReceiveProgressUpdate", new
                            {
                                UploadId = uploadId,
                                Progress = percentage
                            });
                        }
                    }
                }

                var completeRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = _awsConfig.BucketName,
                    Key = filePath,
                    UploadId = s3UploadId,
                    PartETags = partETags
                };

                var completeResponse = await _s3Client.CompleteMultipartUploadAsync(completeRequest);
                var fileUrl = $"https://{_awsConfig.BucketName}.s3.amazonaws.com/{filePath}";

                await _sessionService.SaveFileUploadInfoAsync(new CreateMediaDto
                {
                    Id = SnowflakeIdGeneratorUtil.NextId(),
                    FilePath = fileUrl,
                    FileName = Path.GetFileName(file.FileName),
                    FileType = MediaFileType.Video,
                    SessionId = sessionId,
                    ThumbnailPath = "/thumbnails/sessions/1/Introduction_to_.Net_video1.png",
                    CreatedByUserId = 1,
                    UpdatedByUserId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now.AddMinutes(1),
                    ApprovalStatus = MediaApprovalStatus.Approved,
                });

                return fileUrl;
            }
            catch (Exception ex)
            {
                await _s3Client.AbortMultipartUploadAsync(new AbortMultipartUploadRequest
                {
                    BucketName = _awsConfig.BucketName,
                    Key = filePath,
                    UploadId = s3UploadId
                });

                throw new Exception($"Error uploading file (UploadId: {uploadId}): {ex.Message}", ex);
            }
        }
    }
}
