using AutoMapper;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;

namespace ProductCatalog.Infrastructure.Commands
{
    public class UpdateProductHandler :
        IRequestHandler<UpdateProductWithImagesCommand, ProductDtoWithId>
    {

        protected IProductRepository ProductRepository;
        protected IProductsUnitOfWork Uow;
        protected IMapper Mapper;
        protected IImageStorage ImageStorage;
        public UpdateProductHandler(IProductRepository productRepository,
            IProductsUnitOfWork unitOfWork,
            IMapper mapper,
            IImageStorage imageStorage)
        {
            ProductRepository = productRepository;
            Uow = unitOfWork;
            Mapper = mapper;
            ImageStorage = imageStorage;
        }

        public async Task<ProductDtoWithId> Handle(UpdateProductWithImagesCommand request,
            CancellationToken cancellationToken)
        {
            Product? product = await ProductRepository
                .GetProductByIdAsync(request.Id);

            if (product == null)
                throw new Exception("Unexpected error.");

            var images = new List<ProductImage>();

            try
            {
                await Uow.BeginTransactionAsync();

                product.Title = request.Title;
                product.Description = request.Description;

                await Uow.SaveChangesAsync();

                foreach (var img in product.Images)
                {
                    await ImageStorage.DeleteFileAsync(img.FileName);
                }

                foreach (var file in request.Files)
                {
                    var productImage = new ProductImage();

                    productImage.ProductId = product.Id;

                    productImage.FileName =
                        await ImageStorage.PutFileAsync(file.Content, $"{product.Id}_{file.FileName}");

                    images.Add(productImage);
                }

                await ProductRepository.AddImagesAsync(images);

                await Uow.SaveChangesAsync();

                product.Images = images;

                var productDto = Mapper.Map<Product, ProductDtoWithId>(product);

                await Uow.CommitTransactionAsync();

                return productDto;

            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();

                throw;
            }
        }
    }
}
