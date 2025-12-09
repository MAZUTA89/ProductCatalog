using Minio;
using ProductCatalog.Infrastructure.ImageProcessing;

namespace ProductCatalog.Infrastructure.Data.MinioStorage
{
    public class ProductsImagesStorage : MinioSingleBucketStorage
    {
        const string c_productsBucketName = "products";
        public ProductsImagesStorage(IMinioClient minioClient,
            WebpImageConverter webpImageConverter)
            : base(minioClient,
                  c_productsBucketName,
                  webpImageConverter)
        {
        }
    }
}
