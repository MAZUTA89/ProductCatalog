using ProductCatalog.Infrastructure.Configuring.Helpers.Json;
using ProductCatalog.Infrastructure.Configuring.Interfaces;

namespace ProductCatalog.Infrastructure.Configuring.MinioParams
{
    public class MinioStorageConfig : IMinioConfig
    {
        const string c_rootSectionName = "MinioStorage";
        const string c_endpointName = "endpoint";
        const string c_accessKeyName = "accessKey";
        const string c_secretKeyName = "secretKey";
        const string c_secureName = "secure";
        private JsonConfigHelper _jsonConfigHelper;

        public MinioStorageConfig()
        {
            _jsonConfigHelper = new JsonConfigHelper();
        }
        public void GetMinioParameters(out string endpoint,
            out string accessKey,
            out string secretKey, out bool secure)
        {
            _jsonConfigHelper.SectionExist(c_rootSectionName);
            _jsonConfigHelper.SectionExist(c_endpointName);
            _jsonConfigHelper.SectionExist(c_accessKeyName);
            _jsonConfigHelper.SectionExist(c_secretKeyName);
            _jsonConfigHelper.SectionExist(c_secureName);

            var config = _jsonConfigHelper.GetConfig();

            var rootSection = config.GetSection(c_rootSectionName);

            endpoint = rootSection.GetSection(c_endpointName)
                .Value ?? string.Empty;
            accessKey = rootSection.GetSection(c_accessKeyName)
                .Value ?? string.Empty;
            secretKey = rootSection.GetSection(c_secretKeyName)
                .Value ?? string.Empty;
            secure = bool.Parse(rootSection.GetSection(c_secureName)
                .Value ?? bool.FalseString);
        }
    }
}
