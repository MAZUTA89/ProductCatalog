namespace ProductCatalog.Infrastructure.Configuring.Interfaces
{
    public interface IDbContextConfig
    {
        string GetConnectionString();
    }
}
