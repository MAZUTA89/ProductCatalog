using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDtoWithId>
    {
        public int Id { get; set; }
    }
}
