using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings
{
    public interface IDbContextOption
    {
        DbContextOptionsBuilder GetDbContextOptions();
    }
}
