#nullable disable
namespace ProductCatalog.Application.DTO
{
    public class ProductDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductImageDto> Images { get; set; }
    }
}
