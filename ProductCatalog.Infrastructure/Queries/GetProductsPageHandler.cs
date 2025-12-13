
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;

namespace ProductCatalog.Infrastructure.Queries;

public class GetProductsPageHandler
    : IRequestHandler<GetProductsPageQuery,
        ProductsPage<ProductDtoWithId>>
{
    public GetProductsPageHandler(IProductRepository productRepository,
        IMapper mapper)
    {
        ProductRepository = productRepository;
        Mapper = mapper;
    }

    protected IProductRepository ProductRepository { get; set; }
    protected IMapper Mapper { get; set; }
    public async Task<ProductsPage<ProductDtoWithId>> Handle(
        GetProductsPageQuery request, CancellationToken ct)
    {
        var productsPage = new ProductsPage<ProductDtoWithId>();

        var pageSize = request.PageSize;
        var page = request.Page;

        productsPage.PageSize = pageSize;
        productsPage.Page = page;
        productsPage.Total = await ProductRepository.ProductsCountAsync();
        productsPage.TotalPages = productsPage.Total / request.PageSize;

        var products = await ProductRepository.ProductsQuery()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(p => p.Images)
            .ToListAsync();

        productsPage.Items = Mapper.Map<List<Product>,
            List<ProductDtoWithId>>(products);

        return productsPage;
    }
}
