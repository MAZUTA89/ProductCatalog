using ProductCatalog.Infrastructure.Configuring.Helpers.Json;
using ProductCatalog.Infrastructure.Configuring.Interfaces;

namespace ProductCatalog.Infrastructure.Configuring.DbContext
{
    public class NpgConfigProd : IDbContextConfig
    {
        const string c_npgSubSectionName = "NpgConnectionString";
        const string c_rootSectionName = "ConnectionStrings";

        string? _connectionString;

        public NpgConfigProd()
        {
            JsonConfigHelper.SectionExist(c_rootSectionName);
            JsonConfigHelper.SectionExist(c_npgSubSectionName);

            _connectionString = JsonConfigHelper.Section(c_rootSectionName)
                .GetSection(c_npgSubSectionName).Value;
        }

        public string GetConnectionString()
        {
            return _connectionString!;   
        }
    }
}
