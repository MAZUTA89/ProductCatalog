using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Configuring.Interfaces;

namespace ProductCatalog.Infrastructure.Data.DbContextOptions
{
    public class ProductDbContextOptions : IProductDbContextOption
    {
        IDbContextConfig _dbContextConfig;
        DbContextOptions<ProductDbContext> _options;
        public ProductDbContextOptions(IDbContextConfig dbContextConfig)
        {
            _dbContextConfig = dbContextConfig;

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            builder.UseNpgsql(_dbContextConfig.GetConnectionString());

            _options = builder.Options;
        }
        public DbContextOptions<ProductDbContext> GetDbContextOptions()
        {
            return _options;
        }
    }
}
