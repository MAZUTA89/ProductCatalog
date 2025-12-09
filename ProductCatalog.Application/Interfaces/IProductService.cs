using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateFromMultipartAsync(ProductDto productDto,
            IEnumerable<FileContent> content);
        Task ExportCsvReportAsync(Stream target);
        Task<ProductDto>? GetProductAsync(int id);
        Task<ProductsPage<ProductDto>> GetProductsPageAsync(int page,
            int pageSize);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task RemoveProductAsync(int id);
        Task GetPhotosByProductIdAsync(int id, Stream targetStream);
    }
}
