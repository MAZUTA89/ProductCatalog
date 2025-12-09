namespace ProductCatalog.Infrastructure.Repositories.NpgRepository.Exceptions
{
    public class NpgProductAlreadyExistException : Exception
    {
        NpgRepositoryArgs _args;
        public NpgProductAlreadyExistException(NpgRepositoryArgs args)
            : base($"Product with {args.Title} already exist")
        {
            _args = args;
        }
    }
}
