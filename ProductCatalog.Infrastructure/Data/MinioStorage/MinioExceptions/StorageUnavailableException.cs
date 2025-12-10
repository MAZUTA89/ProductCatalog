namespace ProductCatalog.Infrastructure.Data.MinioStorage.MinioExceptions
{
    public class StorageUnavailableException : Exception
    {
        public StorageUnavailableException()
        {
        }

        public StorageUnavailableException(string? message) : base(message)
        {
        }
    }
}
