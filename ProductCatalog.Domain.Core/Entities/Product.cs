#nullable disable

using ProductCatalog;

namespace ProductCatalog.Domain.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}
