namespace ProductCatalog.Infrastructure.Repositories.NpgRepository.Exceptions
{
    public class NpgProductNotFoundException : Exception
    {
        NpgRepositoryArgs _args;
        public NpgProductNotFoundException(NpgRepositoryArgs args)
            :base($"Product with {args.Id} id not found.")
        {
            _args = args;
        }
    }
}
