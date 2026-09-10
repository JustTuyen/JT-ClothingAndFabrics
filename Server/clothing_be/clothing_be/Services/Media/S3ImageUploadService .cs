using Amazon.S3;
using Amazon.S3.Model;
using SixLabors.ImageSharp;

namespace clothing_be.Services.Media
{
    public class S3ImageUploadService : IImageUploadService
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly string _bucketName;
        private readonly string _region;

        public S3ImageUploadService(IAmazonS3 amazonS3, IConfiguration configuration)
        {
            _amazonS3 = amazonS3;
            _bucketName = configuration["AWS:BucketName"]!;
            _region = configuration["AWS:Region"]!;
        }

        public async Task<UploadResult> UploadAsync(IFormFile file)
        {
            var allowTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowTypes.Contains(file.ContentType))
            {
                throw new InvalidOperationException("Định dạng ảnh không được hỗ trợ.");
            }

            int width;
            int height;
            using (var stream = file.OpenReadStream())
            {
                using var image = await Image.LoadAsync(stream);
                width = image.Width;
                height = image.Height;
            }

            var extension = Path.GetExtension(file.FileName);
            var key = $"Images/{Guid.NewGuid()}{extension}";

            using var uploadStream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = uploadStream,
                ContentType = file.ContentType,
                //CannedACL = S3CannedACL.PublicRead
            };

            await _amazonS3.PutObjectAsync(putRequest);
            var url = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{key}";

            return new UploadResult
            {
                Url = url,
                Key = key,
                Height = height,
                Width = width,
            };
        }

        public async Task DeleteAsync(string key)
        {
            await _amazonS3.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            });
        }
    }
}
