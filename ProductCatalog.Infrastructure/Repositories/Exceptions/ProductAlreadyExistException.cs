namespace ProductCatalog.Infrastructure.Repositories.Exceptions
{
    public class ProductAlreadyExistException : Exception
    {
        ProductsRepositoryArgs _args;
        public ProductAlreadyExistException(ProductsRepositoryArgs args)
            : base($"Product with {args.Title} already exist")
        {
            _args = args;
        }
    }
}
