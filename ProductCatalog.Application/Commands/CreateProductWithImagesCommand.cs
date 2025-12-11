using MediatR;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Commands
{
    public class CreateProductWithImagesCommand : IRequest<ProductDtoWithId>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<FileContent> Files { get; set; }
    }
}
