using Minio;
using ProductCatalog.Infrastructure.ImageProcessing;

namespace ProductCatalog.Infrastructure.Data.MinioStorage
{
    public class ProductsImagesStorage : MinioSingleBucketStorage
    {
        const string c_productsBucketName = "products";
        protected IImageConverter ImageConverter { get; set; }
        public ProductsImagesStorage(IMinioClient minioClient,
            WebpImageConverter imageConverter)
            : base(minioClient,
                  c_productsBucketName)
        {
            ImageConverter = imageConverter;
        }

        public override async Task<string> PutFileAsync(Stream fileStream,
            string fileName)
        {
            Stream webpFile = await ImageConverter.ConvertFromAsync(fileStream);

            var extenstion = Path.GetExtension(fileName);

            fileName = fileName.Replace(extenstion, ".webp");

            await base.PutFileAsync(webpFile, fileName);

            return fileName;
        }
    }
}
