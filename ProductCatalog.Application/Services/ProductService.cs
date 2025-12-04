using ProductCatalog.Application.DTO;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        public ProductService()
        {
            
        }
        public Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
        }

        public Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProductAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
