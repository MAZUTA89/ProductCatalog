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

        public void SectionsExist(string rootSection, params string[] subSections)
        {
            var root = s_config.GetRequiredSection(rootSection);

            string rootName = String.Empty;

            var args = new JsonConfigArgs();

            args.FileName = c_jsonFileName;

            if (root.Exists() == true)
            {
                foreach (var section in subSections)
                {
                    if(root.GetSection(section).Exists() == false)
                    {
                        args.SectionName = $"subsection name: {section}";
                        throw new SectionNotFoundException(args);
                    }
                }
            }
            else
            {
                args.SectionName = $"root section name: {rootSection}";
                throw new SectionNotFoundException(args);
            }
        }

        public IConfigurationRoot GetConfig()
        {
            return s_config; 
        }

    }
}
