using Minio;
using ProductCatalog.Infrastructure.ImageProcessing;

namespace ProductCatalog.Infrastructure.Data.MinioStorage
{
    public class ProductsImagesStorage : MinioSingleBucketStorage
    {
        const string c_productsBucketName = "products";
        protected IImageConverter ImageConverter { get; set; }
        public ProductsImagesStorage(IMinioClient minioClient,
            IImageConverter imageConverter)
            : base(minioClient,
                  c_productsBucketName)
        {
            ImageConverter = imageConverter;
        }

        public override async Task PutFileAsync(Stream fileStream, string fileName)
        {
            Stream webpFile = await ImageConverter.ConvertFromAsync(fileStream);

            await base.PutFileAsync(webpFile, fileName);
        }
    }
}
