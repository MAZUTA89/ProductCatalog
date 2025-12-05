#nullable disable

namespace ProductCatalog.Infrastructure.Configuring.Helpers.Json
{
    public class JsonConfigArgs
    {
        public string SectionName;
        public string FileName;

        public JsonConfigArgs() { }

        public JsonConfigArgs(string sectionName, 
            string fileName)
        {
            SectionName = sectionName;
            FileName = fileName;
        }
    }
}
