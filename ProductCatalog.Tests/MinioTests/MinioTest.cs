using Minio;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Configuring.Interfaces;
using ProductCatalog.Infrastructure.Configuring.MinioParams;
using ProductCatalog.Infrastructure.Data.MinioStorage;

namespace ProductCatalog.Tests.MinioTests
{
    [TestClass]
    public class MinioTest
    {
        [TestMethod]
        public async Task TestMinioClientInitialization()
        {
            try
            {
                IMinioConfig config = new MinioStorageConfig();

                config.GetMinioParameters(
                   out string endpoint,
                   out string accessKey,
                   out string secretKey,
                   out bool secure);

                var minioClient = new MinioClient();

                minioClient.WithEndpoint(endpoint);
                minioClient.WithCredentials(accessKey, secretKey);
                minioClient.WithSSL(secure);
                minioClient.Build();

                IImageStorage st = new ProductsImagesStorage(minioClient, null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            
        }
    }
}
