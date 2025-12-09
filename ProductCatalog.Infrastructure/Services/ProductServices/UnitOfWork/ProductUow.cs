
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductCatalog.Infrastructure.Data.AppDbContext;

namespace ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork
{
    public class ProductUow : IProductsUnitOfWork
    {
        protected IProductDbContext ProductDbContext { get; set; }
        protected ProductDbContextBase ProductDbContextBase { get; set; }

        protected IDbContextTransaction? CurrentTransaction { get; set; }
        protected TransactionStates State { get; set; }

        public ProductUow(ProductDbContextBase dbContext) 
        {
            ProductDbContext = dbContext;
            ProductDbContextBase = dbContext;
            State = TransactionStates.InitializedOnly;
        }
        public virtual async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            CurrentTransaction =
                await ProductDbContext.Database.BeginTransactionAsync();
        }

        public virtual async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            await CurrentTransaction?.CommitAsync();
        }

        public virtual async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            await CurrentTransaction?.RollbackAsync(ct);
        }

        public async Task SaveChangesAsync()
        {
            await ProductDbContextBase.SaveChangesAsync();
        }
    }
}
