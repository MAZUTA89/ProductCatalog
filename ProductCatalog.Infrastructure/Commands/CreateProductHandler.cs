using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.Infrastructure.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductWithImagesCommand, ProductDtoWithId>
    {
        protected IProductRepository ProductRepository;
        protected IProductsUnitOfWork UnitOfWork;
        public CreateProductHandler(IProductRepository productRepository,
            IProductsUnitOfWork unitOfWork)
        {
            ProductRepository = productRepository;
            UnitOfWork = unitOfWork;
        }

        public Task<ProductDtoWithId> Handle(CreateProductWithImagesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
