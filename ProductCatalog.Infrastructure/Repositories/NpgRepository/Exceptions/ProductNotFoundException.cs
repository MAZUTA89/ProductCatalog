namespace ProductCatalog.Infrastructure.Repositories.NpgRepository.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        NpgRepositoryArgs _args;
        public ProductNotFoundException(NpgRepositoryArgs args)
            :base($"Product with {args.Id} id not found.")
        {
            _args = args;
        }
    }
}
