#nullable disable

namespace ProductCatalog.Infrastructure.Repositories.NpgRepository.Exceptions
{
    public class NpgRepositoryArgs
    {
        public int Id;
        public string Title;
        public NpgRepositoryArgs()
        {
            
        }
        public NpgRepositoryArgs(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
