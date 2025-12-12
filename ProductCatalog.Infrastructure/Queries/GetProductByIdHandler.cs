using AutoMapper;
using MediatR;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;

namespace ProductCatalog.Infrastructure.Queries
{
    public class GetProductByIdHandler :
        IRequestHandler<GetProductByIdQuery, ProductDtoWithId>
    {
        protected IProductRepository ProductRepository { get; set; }
        protected IMapper Mapper { get; set; }
        public GetProductByIdHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            ProductRepository = productRepository;
            Mapper = mapper;
        }

        public async Task<ProductDtoWithId> Handle(GetProductByIdQuery request,
            CancellationToken ct)
        {
            if (request.Id < 0)
                return default;

            var product = await ProductRepository
                .GetProductByIdAsync(request.Id);

            if (product == null)
                return default;

            return Mapper.Map<Product, ProductDtoWithId>(product);
        }
    }
}
