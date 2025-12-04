#nullable disable

namespace ProductCatalog.Application.DTOs
{
    public class ProductCsvDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ImagesAmount { get; set; }
    }
}
