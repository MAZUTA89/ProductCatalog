#nullable disable
namespace ProductCatalog.Application.DTOs
{
    public class FileContent
    {
        public string FileName;
        public Stream Content;

        public FileContent()
        {
            
        }

        public FileContent(string fileName, Stream content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
