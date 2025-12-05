using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Infrastructure.Data
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> Images { get; set; }

        private ProductTypeConfiguration _productEntityConfig;
        private ProductImgTypeConfiguration _productImgEntityConfig;

        public ProductDbContext()
        {
            _productEntityConfig = new ProductTypeConfiguration();
            _productImgEntityConfig = new ProductImgTypeConfiguration();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(_productEntityConfig);
            modelBuilder.ApplyConfiguration(_productImgEntityConfig);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ProductCatalogConfig("appsettings.json");

            optionsBuilder.UseNpgsql(config.GetConnectionString());
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public async Task<bool> CanConnectAsync()
        {
            return await Database.CanConnectAsync();
        }

    }
}
