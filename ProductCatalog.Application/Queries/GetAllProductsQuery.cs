using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Queries;

public record GetProductsQuery : IRequest<IEnumerable<ProductDtoWithId>>;

