using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.Configuring.DbContext;

namespace ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings
{
    public class NpgContextOptionsFacade : DbContextOptionsBase, IDbContextOption
    {
        public NpgContextOptionsFacade(NpgConfigProd config)
            : base(config) { }
        
        public override DbContextOptionsBuilder GetDbContextOptions()
        {
            return new DbContextOptionsBuilder()
                .UseNpgsql(Config.GetConnectionString());
        }
    }
}
