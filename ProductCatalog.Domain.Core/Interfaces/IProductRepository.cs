using System;
using System.Collections.Generic;
using ProductCatalog.Domain.Core.Entities;


namespace ProductCatalog.Domain.Core.Interfaces
{
    public interface IProductRepository : IDisposable, IAsyncDisposable
    {
        IQueryable<Product> ProductsQuery();
        Product? GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<int> CountAsync();
    }
}
