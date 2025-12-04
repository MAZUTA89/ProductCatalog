using System;
using System.Collections.Generic;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateFromMilttpartAsync(string json,
            IEnumerable<Stream> content);
        Task<Stream> GetCsvReport();
        Task<ProductDto> GetProductAsync(int id);
        Task<ProductsPage<ProductDto>> GetProductsPageAsync(int page,
            int pageSize);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(ProductDto productDto);
        Task RemoveProductAsync(ProductDto productDto);

    }
}
