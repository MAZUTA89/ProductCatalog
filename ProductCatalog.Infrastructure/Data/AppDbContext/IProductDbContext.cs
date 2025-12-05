using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public interface IProductDbContext : IDisposable, IAsyncDisposable
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DatabaseFacade DbContext {  get; }
    }
}
