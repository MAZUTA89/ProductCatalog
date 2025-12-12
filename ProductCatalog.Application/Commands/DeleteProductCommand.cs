using MediatR;

namespace ProductCatalog.Application.Commands;

public class DeleteProductCommand : IRequest<string>
{
    public int Id { get; set; }
}
