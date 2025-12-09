using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;
using ProductCatalog.Infrastructure.Data.AppDbContext.EntityTypeConfigurations;

namespace ProductCatalog.Infrastructure.Data.AppDbContext
{
    public class NpgProductDbContext : ProductDbContextBase
    {
        public NpgProductDbContext(NpgContextOptionsFacade dbContextOption)
            : base(dbContextOption)
        {
            ProductTableConfig = new ProductTypeConfiguration();
            ImageTableConfig = new ProductImgTypeConfiguration();
        }

    }
}
