using System;
using Microsoft.EntityFrameworkCore.Design;
using ProductCatalog.Infrastructure.Configuring.DbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;

namespace ProductCatalog.Infrastructure.Data
{
    public class NpgProductDbContextFactoryMigrations : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            NpgConfigProd config = new();

            NpgContextOptionsFacade option = new(config);

            ProductDbContext dbContext = new ProductDbContext(option);

            return dbContext;
        }
    }
}
