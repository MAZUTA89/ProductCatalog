
namespace ProductCatalog.Domain.Core.Interfaces
{
    public interface IImageStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fileName"></param>
        /// <returns>Returns added file name</returns>
        Task<string> PutFileAsync(Stream fileStream, string fileName);
        Task DeleteFileAsync(string fileName);
        Task<Stream> GetFileAsync(string fileName);
    }
}
