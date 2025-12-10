using System;
using System.Collections.Generic;
using ProductCatalog.Domain.Core.Entities;


namespace ProductCatalog.Domain.Core.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> ProductsQuery();
        IQueryable<ProductImage> ImagesQuery();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<IEnumerable<ProductImage>> AddImagesAsync(IEnumerable<ProductImage> images);
        Task<ProductImage> AddImageAsync(ProductImage image);
        Task DeleteProductAsync(int id);
        Task<int> ProductsCountAsync();
        Task<int> ImagesCountAsync(int productId);
    }
}
