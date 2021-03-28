using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Hermes.API.Media.Domain.Constants;
using Hermes.API.Media.Domain.Models;
using Hermes.API.Media.Domain.Requests;

namespace Hermes.API.Media.Domain.Services
{
    public class ImageService : IImageService
    {
        public async Task<ImageDto> Upload(UploadImageRequest uploadImageRequest)
        {
            var accessKey = Environment.GetEnvironmentVariable(ConfigConstants.AwsAccessKey);
            var secretKey = Environment.GetEnvironmentVariable(ConfigConstants.AwsSecretKey);
            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
            };
            using var client = new AmazonS3Client(credentials, config);
            var file = Convert.FromBase64String(uploadImageRequest.Base64String);
            await using var newMemoryStream = new MemoryStream(file);
            var imageKey = Guid.NewGuid();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = imageKey.ToString(),
                BucketName = StorageConstants.DefaultBucketName,
                CannedACL = S3CannedACL.PublicRead
            };
            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            return new ImageDto()
            {
                Id = imageKey,
                DisplayOrder = 0,
                ImageUrl = $"{StorageConstants.DefaultBucketUrl}/{imageKey}",
                IsDeleted = false,
                ThumbnailUrl = $"{StorageConstants.DefaultBucketUrl}/{imageKey}"
            };
        }
    }
}