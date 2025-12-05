using System;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Infrastructure.Configuring.Helpers.Json
{
    public class JsonConfigHelper
    {
        const string c_jsonFileName = "appsettings.json";
        private static IConfigurationRoot s_config;

        public JsonConfigHelper()
        {
            s_config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(c_jsonFileName)
                .Build();
        }

        public static void SectionExist(string sectionName)
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

        public static IConfigurationSection Section(string sectionName)
        {
            SectionExist(sectionName);

            return s_config.GetRequiredSection(sectionName);
        }

    }
}
