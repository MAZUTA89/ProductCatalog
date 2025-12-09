using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public class ProductDbContextBase : DbContext, IProductDbContext 
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> Images { get; set; }

        public DatabaseFacade Database => base.Database;

        protected IEntityTypeConfiguration<Product> ProductTableConfig { get; set; }

        protected IEntityTypeConfiguration<ProductImage> ImageTableConfig { get; set; }

        public ProductDbContextBase(IDbContextOption dbContextOption)
            :base(dbContextOption.GetDbContextOptions().Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(ProductTableConfig);
            modelBuilder.ApplyConfiguration(ImageTableConfig);
        }
    }
}
