using Microsoft.Extensions.Configuration;
using ProductCatalog.Infrastructure.Configuring.Helpers.Json;
using ProductCatalog.Infrastructure.Configuring.Interfaces;

namespace ProductCatalog.Infrastructure.Configuring.DbContext
{
    public class NpgConfigProd : IDbContextConfig
    {
        const string c_npgSubSectionName = "NpgConnectionString";
        const string c_rootSectionName = "ConnectionStrings";

        string _connectionString;
        JsonConfigHelper _configHelper;

        public NpgConfigProd()
        {
            _configHelper = new JsonConfigHelper();

            _connectionString = String.Empty;
        }

        public string GetConnectionString()
        {
            _configHelper.SectionExist(c_rootSectionName);
            _configHelper.SectionExist(c_npgSubSectionName);

            var config = _configHelper.GetConfig();

            _connectionString = 
                config.GetRequiredSection(c_rootSectionName)
                .GetSection(c_npgSubSectionName).Value ?? String.Empty;

            return _connectionString;   
        }
    }
}
