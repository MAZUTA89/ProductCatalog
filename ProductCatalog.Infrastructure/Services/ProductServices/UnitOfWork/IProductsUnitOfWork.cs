namespace ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork
{
    public interface IProductsUnitOfWork
    {
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
        Task SaveChangesAsync();
    }
}
