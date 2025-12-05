using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public abstract class ProductDbContextBase : DbContext, IProductDbContext 
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> Images { get; set; }

        public DatabaseFacade DbContext => Database;

        public abstract IEntityTypeConfiguration<Product> ProductTableConfig { get; }
        public abstract IEntityTypeConfiguration<ProductImage> ImageTableConfig { get; }

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
