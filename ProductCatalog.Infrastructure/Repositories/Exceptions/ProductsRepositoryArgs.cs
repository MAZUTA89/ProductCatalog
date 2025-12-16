#nullable disable

using ProductCatalog;

namespace ProductCatalog.Infrastructure.Repositories.Exceptions
{
    public class ProductsRepositoryArgs
    {
        public int Id;
        public string Title;
        public ProductsRepositoryArgs()
        {
            
        }
        public ProductsRepositoryArgs(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
