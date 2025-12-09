namespace ProductCatalog.Infrastructure.ImageProcessing
{
    public interface IImageConverter
    {
        Task<Stream> ConvertFromAsync(Stream stream);
    }
}
