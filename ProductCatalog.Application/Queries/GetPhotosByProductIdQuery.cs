using MediatR;

namespace ProductCatalog.Application.Queries;

public class GetPhotosByProductIdQuery : IRequest<Stream>
{
    public int Id { get; set; }
}
