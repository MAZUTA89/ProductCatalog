using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries
{
    public class GetProductsPageQuery 
        : IRequest<ProductsPage<ProductDtoWithId>>
    {
        public int Page;
        public int PageSize { get; set; }
    }
}
