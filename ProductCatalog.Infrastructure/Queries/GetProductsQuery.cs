using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;

namespace ProductCatalog.Infrastructure.Queries;

public class GetProductsHandler : IRequestHandler<GetProductsQuery,
    IEnumerable<ProductDtoWithId>>
{
    public GetProductsHandler(IProductRepository productRepository,
        IMapper mapper)
    {
        ProductRepository = productRepository;
        Mapper = mapper;
    }

    protected IProductRepository ProductRepository { get; set; }
    protected IMapper Mapper { get; set; }

    public async Task<IEnumerable<ProductDtoWithId>> Handle(
        GetProductsQuery request, CancellationToken ct)
    {
        var products = await ProductRepository.ProductsQuery()
            .Include(p => p.Images)
            .ToListAsync();

        var productsDto = Mapper.Map<List<Product>,
            List<ProductDtoWithId>>(products);

        return productsDto;
    }
}
