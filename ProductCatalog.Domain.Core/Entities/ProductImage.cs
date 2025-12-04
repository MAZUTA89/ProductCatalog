#nullable disable
namespace ProductCatalog.Domain.Core.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string FileName { get; set; }
    }
}
