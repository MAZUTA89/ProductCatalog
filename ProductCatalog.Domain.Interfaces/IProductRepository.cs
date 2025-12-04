using ProductCatalog.Domain.Core.Entities;
using System;
using System.Collections.Generic;


namespace ProductCatalog.Domain.Interfaces
{
    public interface IProductRepository : IDisposable, IAsyncDisposable
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task Save();
    }
}
