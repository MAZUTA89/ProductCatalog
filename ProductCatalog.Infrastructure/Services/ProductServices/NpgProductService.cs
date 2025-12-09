using AutoMapper;
using ProductCatalog.Infrastructure.Data.MinioStorage;
using ProductCatalog.Infrastructure.Repositories.NpgRepository;
using ProductCatalog.Infrastructure.Services.ProductServices.Abstructions;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.Infrastructure.Services.ProductServices
{
    public class NpgProductService : ProductService
    {
        public NpgProductService(NpgProductRepository productRepository,
             ProductsImagesStorage imageStorage, ProductUow uow, IMapper mapper)
            : base(productRepository, imageStorage, uow, mapper)
        {
        }
    }
}
