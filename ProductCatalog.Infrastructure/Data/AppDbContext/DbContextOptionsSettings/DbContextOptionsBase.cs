using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Configuring.Interfaces;

namespace ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings
{
    public abstract class DbContextOptionsBase : IDbContextOption
    {
        protected IDbContextConfig Config;
        protected DbContextOptions DbContextOptions;
        public DbContextOptionsBase(IDbContextConfig dbContextConfig)
        {
            Config = dbContextConfig;
        }

        public abstract DbContextOptionsBuilder GetDbContextOptions();
        
    }
}
