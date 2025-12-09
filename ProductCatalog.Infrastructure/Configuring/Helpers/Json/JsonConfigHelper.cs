using System;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Infrastructure.Configuring.Helpers.Json
{
    public class JsonConfigHelper
    {
        const string c_jsonFileName = "appsettings.json";
        private IConfigurationRoot s_config;

        public JsonConfigHelper()
        {
            string dir = Directory.GetCurrentDirectory();
            s_config = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile(c_jsonFileName)
                .Build();
        }

        public void SectionExist(string sectionName)
        {
            if(s_config.GetSection(sectionName).Exists() == false)
            {
                JsonConfigArgs args = new()
                {
                    FileName = c_jsonFileName,
                    SectionName = sectionName
                };

                throw new SectionNotFoundException(args);
            }
        }

        public IConfigurationRoot GetConfig()
        {
            return s_config; 
        }

    }
}
