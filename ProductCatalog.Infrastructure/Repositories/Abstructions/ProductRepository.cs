using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Data.AppDbContext;
using ProductCatalog.Infrastructure.Repositories.Exceptions;

namespace ProductCatalog.Infrastructure.Repositories.Abstructions
{
    public class ProductRepository : IProductRepository
    {
        protected IProductDbContext ProductDbContext;
        public ProductRepository(IProductDbContext dbContext)
        {
            ProductDbContext = dbContext;
        }

        public virtual async Task<Product> AddProductAsync(Product product)
        {
            if (await ProductDbContext.Products.AnyAsync(
                p => p.Title == product.Title))
            {
                var args = new ProductsRepositoryArgs();
                args.Title = product.Title;

                throw new ProductAlreadyExistException(args);
            }

            await ProductDbContext.Products.AddAsync(product);

            return product;
        }

        public virtual async Task<int> ProductsCountAsync()
        {
            return await ProductDbContext.Products.CountAsync();
        }

        public virtual async Task DeleteProductAsync(int id)
        {
            var product = await ProductDbContext
                .Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                var args = new ProductsRepositoryArgs()
                {
                    Id = id
                };

                throw new ProductNotFoundException(args);
            }

            ProductDbContext.Products.Remove(product);
        }

        public virtual async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await ProductDbContext
                .Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                var args = new ProductsRepositoryArgs()
                {
                    Id = id
                };

                throw new ProductNotFoundException(args);
            }

            return product;
        }

        public virtual IQueryable<Product> ProductsQuery()
        {
            return ProductDbContext.Products.AsQueryable();
        }

        public IQueryable<ProductImage> ImagesQuery()
        {
            return ProductDbContext.Images.AsQueryable();
        }

        public virtual async Task<int> ImagesCountAsync(int productId)
        {
            return await ProductDbContext.Products.CountAsync();
        }

        public async Task<IEnumerable<ProductImage>> AddImagesAsync(
            IEnumerable<ProductImage> images)
        {
            await ProductDbContext.Images.AddRangeAsync(images);

            return images;
        }

        public async Task<ProductImage> AddImageAsync(ProductImage image)
        {
            if(await ProductDbContext.Images.AnyAsync(
                i => i.FileName == image.FileName))
            {
                var args = new ProductsRepositoryArgs();
                args.Title = image.FileName;

                throw new ProductAlreadyExistException(args);
            }
            
            await ProductDbContext.Images.AddAsync(image);

            return image;
        }
    }
}
