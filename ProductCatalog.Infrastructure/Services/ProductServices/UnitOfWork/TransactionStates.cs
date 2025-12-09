namespace ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork
{
    public enum TransactionStates
    {
        Begin,
        Commit,
        Rollback,
        InitializedOnly
    }
}
