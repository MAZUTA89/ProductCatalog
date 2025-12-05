using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Infrastructure.Data.DbContextOptions
{
    public interface IProductDbContextOption
    {
        DbContextOptions<ProductDbContext> GetDbContextOptions();
    }
}
