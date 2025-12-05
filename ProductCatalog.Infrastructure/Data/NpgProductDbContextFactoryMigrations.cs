using System;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductCatalog.Infrastructure.Data
{
    public class NpgProductDbContextFactoryMigrations : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {

        }
    }
}
