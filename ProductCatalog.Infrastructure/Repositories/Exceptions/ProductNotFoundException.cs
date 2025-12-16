namespace ProductCatalog.Infrastructure.Repositories.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        ProductsRepositoryArgs _args;
        public ProductNotFoundException(ProductsRepositoryArgs args)
            :base($"Product with {args.Id} id not found.")
        {
            _args = args;
        }
    }
}
