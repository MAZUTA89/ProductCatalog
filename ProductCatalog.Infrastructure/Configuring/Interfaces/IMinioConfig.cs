namespace ProductCatalog.Infrastructure.Configuring.Interfaces
{
    public interface IMinioConfig
    {
        void GetMinioParameters(out string endpoint,
            out string accessKey,
            out string secretKey, out bool secure);
    }
}
