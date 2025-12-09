using ProductCatalog.Infrastructure.Data.AppDbContext;
using ProductCatalog.Infrastructure.Repositories.Abstructions;

namespace ProductCatalog.Infrastructure.Repositories.NpgRepository
{
    public class NpgProductRepository : ProductRepository
    {
        NpgProductDbContext _dbContext;
        public NpgProductRepository(NpgProductDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
