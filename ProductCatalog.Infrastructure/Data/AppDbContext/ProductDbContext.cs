using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;
using ProductCatalog.Infrastructure.Data.AppDbContext.EntityTypeConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public class ProductDbContext : ProductDbContextBase
    {
        public override IEntityTypeConfiguration<Product> ProductTableConfig =>
            _productConfig;

        public override IEntityTypeConfiguration<ProductImage> ImageTableConfig =>
            _productImgConfig;

        private ProductTypeConfiguration _productConfig;
        private ProductImgTypeConfiguration _productImgConfig;

        public ProductDbContext(NpgContextOptionsFacade dbContextOption)
            : base(dbContextOption)
        {
            _productConfig = new ProductTypeConfiguration();
            _productImgConfig = new ProductImgTypeConfiguration();

            Database.EnsureCreated();
        }

    }
}
