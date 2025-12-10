using Minio;
using Minio.DataModel.Args;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.ImageProcessing;
using System.Net.Mime;

namespace ProductCatalog.Infrastructure.Data.MinioStorage
{
    public abstract class MinioSingleBucketStorage : IImageStorage
    {
        protected string BucketName;

        protected IMinioClient MinioClient;
        protected IImageConverter? ImageConverter;

        public MinioSingleBucketStorage(IMinioClient minioClient,
            string bucketName)
        {
            BucketName = bucketName;

            MinioClient = minioClient;
        }

        public virtual async Task DeleteFileAsync(string fileName)
        {
            await EnsureBucketExistAsync(BucketName);

            string contentType = GetContentType(fileName);

            var removeObjArgs = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName);

            await MinioClient.RemoveObjectAsync(removeObjArgs);
        }

        public virtual async Task<Stream> GetFileAsync(string fileName)
        {
            await EnsureBucketExistAsync(BucketName);

            var contentType = GetContentType(fileName);

            MemoryStream ms = new MemoryStream();

            var downloadObjArgs = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName)
                .WithCallbackStream(async stream => await stream.CopyToAsync(ms));

            await MinioClient.GetObjectAsync(downloadObjArgs);

            ms.Position = 0;

            return ms;
        }

        public virtual async Task PutFileAsync(Stream fileStream, string fileName)
        {
            await EnsureBucketExistAsync(BucketName);

            string contentType = GetContentType(fileName);

            var putObjArgs = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await MinioClient.PutObjectAsync(putObjArgs);
        }

        public virtual async Task EnsureBucketExistAsync(string bucketName)
        {
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs();
            bucketExistsArgs.WithBucket(bucketName);

            bool isBucketExist =
                await MinioClient.BucketExistsAsync(bucketExistsArgs);

            if (isBucketExist == false)
            {
                var bucketArgs = new MakeBucketArgs();
                bucketArgs.WithBucket(bucketName);

                await MinioClient.MakeBucketAsync(bucketArgs);
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            return
                (extension == ".jpg") ? MediaTypeNames.Image.Jpeg :
                (extension == ".png") ? MediaTypeNames.Image.Png :
                (extension == ".jpeg") ? MediaTypeNames.Image.Jpeg :
                (extension == ".webp") ? MediaTypeNames.Image.Webp :
                MediaTypeNames.Application.Octet;
        }
    }
}
