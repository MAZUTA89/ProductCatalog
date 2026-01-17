using ProductCatalog.Infrastructure.Configuring.DbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext;
using ProductCatalog.Infrastructure.Data.AppDbContext.DbContextOptionsSettings;

namespace ProductCatalog.Tests.DbContextTests
{
    [TestClass]
    public class DbContextTests
    {
        [TestMethod]
        public void CreateNpgProductDbContext()
        {
            try
            {
                NpgConfigProd configProd = new NpgConfigProd();

                NpgContextOptionsFacade npgOption =
                    new NpgContextOptionsFacade(configProd);

                NpgProductDbContext dbContext = new NpgProductDbContext(npgOption);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
