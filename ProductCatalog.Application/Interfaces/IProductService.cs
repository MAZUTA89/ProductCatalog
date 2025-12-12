using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateFromMultipartAsync(CreateProductCommand createProductData,
            IEnumerable<FileContent> content);
        Task<ProductDto> CreateProductAsync(CreateProductCommand createProductData);
        Task<ProductDtoWithId?> GetProductAsync(int id);
        Task<ProductsPage<ProductDtoWithId>> GetProductsPageAsync(int page,
            int pageSize);
        Task<IEnumerable<ProductDtoWithId>> GetAllProductsAsync();
        Task<ProductDto> GetPhotosByProductIdAsync(int id, Stream targetStream);
        Task<ProductDto> RemoveProductAsync(int id);
        Task UpdateProductAsync(UpdateProductCommand productDto);
        Task ExportCsvReportAsync(Stream target);
    }
}
