using AutoMapper;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.Infrastructure.Commands
{
    public class CreateProductHandler : IRequestHandler<CreateProductWithImagesCommand, ProductDtoWithId>
    {
        protected IProductRepository ProductRepository;
        protected IProductsUnitOfWork Uow;
        protected IMapper Mapper;
        protected IImageStorage ImageStorage;
        public CreateProductHandler(IProductRepository productRepository,
            IProductsUnitOfWork unitOfWork,
            IMapper mapper,
            IImageStorage imageStorage)
        {
            ProductRepository = productRepository;
            Uow = unitOfWork;
            Mapper = mapper;
            ImageStorage = imageStorage;
        }

        public virtual async Task<ProductDtoWithId> Handle(
            CreateProductWithImagesCommand request,
            CancellationToken ct)
        {

            var content = request.Files;

            var product = Mapper.Map<CreateProductWithImagesCommand,
                Product>(request);

            var images = new List<ProductImage>();

            try
            {
                await Uow.BeginTransactionAsync();

                product = await ProductRepository.AddProductAsync(product);

                await Uow.SaveChangesAsync();

                foreach (var file in content)
                {
                    var productImage = new ProductImage();

                    productImage.ProductId = product.Id;

                    productImage.FileName =
                        await ImageStorage.PutFileAsync(file.Content,
                        $"{product.Id}_{Guid.NewGuid()}_{file.FileName}");

                    images.Add(productImage);
                }

                HashSet<string> namesHash = new HashSet<string>(images.Select(i=> i.FileName));

                if (namesHash.Count != images.Count)
                {
                    foreach (var name in images)
                    {
                        Console.WriteLine(name);
                    }
                    throw new Exception("Беда");
                }

                await ProductRepository.AddImagesAsync(images);

                await Uow.SaveChangesAsync();

                await Uow.CommitTransactionAsync();

                var productDto = Mapper.Map<Product, ProductDtoWithId>(product);

                return productDto;
            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();

                foreach (var img in images)
                {
                    await ImageStorage.DeleteFileAsync(img.FileName);
                }
                throw;
            }
        }
    }
}
