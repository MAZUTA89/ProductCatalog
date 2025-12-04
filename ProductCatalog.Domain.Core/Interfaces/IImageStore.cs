
namespace ProductCatalog.Domain.Core.Interfaces
{
    public interface IImageStore
    {
        Task PutFileAsync(Stream fileStream, string fileName);
        Task DeleteFileAsync(string fileName);
        Task<Stream> GetFileAsync(string fileName);
    }
}
