using System;
using Microsoft.EntityFrameworkCore.Design;
using ProductCatalog.Infrastructure.Configuring.DbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public class NpgProductDbContextFactoryMigrations : IDesignTimeDbContextFactory<NpgProductDbContext>
    {
        public NpgProductDbContext CreateDbContext(string[] args)
        {
            NpgConfigProd config = new();

            NpgContextOptionsFacade option = new(config);

            NpgProductDbContext dbContext = new NpgProductDbContext(option);

            return dbContext;
        }
    }
}
