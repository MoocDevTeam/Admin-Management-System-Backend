using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Amazon;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;

namespace Mooc.Application.Admin
{

    //public FileUploadService(AwsS3Config awsConfig)
    //{
    //    _awsConfig = awsConfig;
    //    _s3Client = new AmazonS3Client(_awsConfig.AccessKeyId, _awsConfig.SecretAccessKey, RegionEndpoint.GetBySystemName(_awsConfig.Region));
    //}
    public class AvatarService : IAvatarService, IScopedDependency
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3Config _avatarAwsConfig;

        public AvatarService(AwsS3Config avatarAwsConfig)
        {
            _avatarAwsConfig = avatarAwsConfig;
            _s3Client = new AmazonS3Client(
                _avatarAwsConfig.AccessKeyId,
                _avatarAwsConfig.SecretAccessKey,
                Amazon.RegionEndpoint.GetBySystemName(_avatarAwsConfig.Region)
            );
        }

        public async Task<string> UploadAvatarAsync(string userName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is required.");
            }

            const long maxFileSize = 2 * 1024 * 1024; // 2MB
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("File size exceeds the 2MB limit.");
            }

            string[] allowedFormats = { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedFormats.Contains(fileExtension))
            {
                throw new ArgumentException("Invalid file format. Only JPG, JPEG, PNG are allowed.");
            }

            var key = $"{userName}.png";

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = _avatarAwsConfig.BucketName,
                        Key = key,
                        InputStream = stream,
                        ContentType = "image/png",
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await _s3Client.PutObjectAsync(request);
                }

                return $"https://{_avatarAwsConfig.BucketName}.s3.{_avatarAwsConfig.Region}.amazonaws.com/{key}";
            }
            catch (Exception ex)
            {
                // Handle exception (you can throw or return a response if needed)
                return $"Error uploading avatar: {ex.Message}";
            }
        }

        public async Task DeleteAvatarAsync(string userName)
        {
            var key = $"{userName}.png";

            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _avatarAwsConfig.BucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(request);
            }
            catch (Exception ex)
            {
                // Optionally return a failure response, or handle the exception
                throw new Exception($"Error deleting avatar for user {userName}: {ex.Message}");
            }
        }

        public async Task<string> GetAvatarUrlAsync(string userName)
        {
            var key = $"{userName}.png";
            var avatarUrl = $"https://{_avatarAwsConfig.BucketName}.s3.{_avatarAwsConfig.Region}.amazonaws.com/{key}";

            try
            {
                await _s3Client.GetObjectMetadataAsync(_avatarAwsConfig.BucketName, key);
                return avatarUrl; // Return avatar URL if file exists
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; // Return null if avatar does not exist
            }
            catch (Exception ex)
            {
                // Optionally return a message or handle the error differently
                throw new Exception($"Error retrieving avatar at {avatarUrl}: {ex.Message}");
            }
        }
    }


}

