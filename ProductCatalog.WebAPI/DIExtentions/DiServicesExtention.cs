using Minio;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Mappers;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Configuring.DbContext;
using ProductCatalog.Infrastructure.Configuring.Interfaces;
using ProductCatalog.Infrastructure.Configuring.MinioParams;
using ProductCatalog.Infrastructure.Data.AppDbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;
using ProductCatalog.Infrastructure.Data.MinioStorage;
using ProductCatalog.Infrastructure.ImageProcessing;
using ProductCatalog.Infrastructure.Repositories.Abstructions;
using ProductCatalog.Infrastructure.Repositories.NpgRepository;
using ProductCatalog.Infrastructure.Services.ProductServices;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.WebAPI.Extentions
{
    public static class DiServicesExtention
    {
        public static IServiceCollection AddProductServices
            (this IServiceCollection services)
        {
            services.AddScoped<IProductsUnitOfWork, ProductUow>();
            services.AddScoped<ProductUow>();
            services.AddScoped<IProductService, NpgProductService>();
            return services;
        }
        public static IServiceCollection AddNpgProductRepository
            (this IServiceCollection services)
        {
            services.AddSingleton<NpgConfigProd>();

            services.AddSingleton<NpgContextOptionsFacade>();

            services.AddSingleton<IDbContextOption, NpgContextOptionsFacade>();

            services.AddScoped<NpgProductDbContext>();

            services.AddScoped<IProductDbContext, NpgProductDbContext>();

            services.AddScoped<ProductDbContextBase>(sp => sp.GetRequiredService<NpgProductDbContext>());

            services.AddScoped<ProductRepository, NpgProductRepository>();

            services.AddScoped<NpgProductRepository>();

            services.AddScoped<IProductRepository, NpgProductRepository>();
            
            return services;
        }

        public static IServiceCollection AddImageStorage
            (this IServiceCollection services)
        {
            services.AddSingleton<IMinioConfig, MinioStorageConfig>();
            services.AddSingleton<MinioStorageConfig>();

            services.AddSingleton<IMinioClient>(factory =>
            {
                var config = factory.GetRequiredService<MinioStorageConfig>();

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

                return minioClient;
            });

            services.AddTransient<WebpImageConverter>();

            services.AddSingleton<IImageStorage, ProductsImagesStorage>();
            services.AddSingleton<ProductsImagesStorage>();

            return services;
        }

        public static IServiceCollection AddMapper
            (this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(ProductProfile));

            return services;
        }
    }
}
