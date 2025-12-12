using AutoMapper;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.Infrastructure.Commands;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand,
    string>
{
    protected IProductRepository ProductRepository { get; set; }
    protected IMapper Mapper;
    protected IProductsUnitOfWork Uow;

    public DeleteProductHandler(IProductRepository productRepository,
        IMapper mapper,
        IImageStorage imageStorage)
    {
        ProductRepository = productRepository;
        Mapper = mapper;
        ImageStorage = imageStorage;
    }

    protected IImageStorage ImageStorage { get; set; }

    public async Task<string> Handle(DeleteProductCommand request,
        CancellationToken ct)
    {
        try
        {
            var product = await ProductRepository
                .GetProductByIdAsync(request.Id);

            if (product == null)
            {
                throw new Exception("Unexpected error.");
            }

            foreach (var img in product.Images)
            {
                await ImageStorage.DeleteFileAsync(img.FileName);
            }

            await Uow.BeginTransactionAsync();

            await ProductRepository.DeleteProductAsync(request.Id);

            await Uow.SaveChangesAsync();

            await Uow.CommitTransactionAsync();

            return $"Product with id: {request.Id} deleted.";
        }
        catch (Exception ex)
        {
            await Uow.RollbackTransactionAsync();
            throw;
        }
    }
}
