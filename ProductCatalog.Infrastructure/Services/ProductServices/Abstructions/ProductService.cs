using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Services.ProductServices.UnitOfWork;
using System.Globalization;
using System.IO.Compression;
using System.Text;


namespace ProductCatalog.Infrastructure.Services.ProductServices.Abstructions
{
    public abstract class ProductService : IProductService
    {
        protected IProductRepository ProductRepository { get; set; }
        protected IImageStorage ImageStore { get; set; }
        protected IMapper Mapper { get; set; }
        protected IProductsUnitOfWork Uow { get; set; }
        protected ProductService(IProductRepository productRepository,
            IImageStorage imageStore, IProductsUnitOfWork uow,
            IMapper mapper)
        {
            ProductRepository = productRepository;
            ImageStore = imageStore;
            Uow = uow;
            Mapper = mapper;
        }

        public virtual async Task<ProductDto> CreateFromMultipartAsync(
            CreateProductCommand createProductData,
            IEnumerable<FileContent> content)
        {
            var product = Mapper.Map<CreateProductCommand, Product>(createProductData);

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
                        await ImageStore.PutFileAsync(file.Content, $"{product.Id}_{file.FileName}");

                    images.Add(productImage);
                }

                await ProductRepository.AddImagesAsync(images);

                await Uow.SaveChangesAsync();

                await Uow.CommitTransactionAsync();

                var productDto = Mapper.Map<Product, ProductDto>(product);

                return productDto;
            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();

                foreach (var img in images)
                {
                    await ImageStore.DeleteFileAsync(img.FileName);
                }
                throw;
            }
        }

        public virtual async Task<ProductDto> CreateProductAsync(
            CreateProductCommand createProductData)
        {
            var product = Mapper.Map<CreateProductCommand, Product>(createProductData);

            try
            {
                await Uow.BeginTransactionAsync();

                product = await ProductRepository.AddProductAsync(product);

                var productDto = Mapper.Map<Product, ProductDto>(product);

                await Uow.SaveChangesAsync();

                await Uow.CommitTransactionAsync();

                return productDto;
            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();
                throw;
            }

        }

        public virtual async Task<IEnumerable<ProductDtoWithId>> GetAllProductsAsync()
        {
            var products = await ProductRepository.ProductsQuery()
                .Include(p => p.Images)
                .ToListAsync();

            var productsDto = Mapper.Map<List<Product>,
                List<ProductDtoWithId>>(products);

            return productsDto;
        }

        public virtual async Task ExportCsvReportAsync(Stream targetStream)
        {
            await using var writer = new StreamWriter(
                targetStream,
                Encoding.UTF8);

            await using var csvWriter = new CsvWriter(writer,
                CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<ProductCsvDto>();
            await csvWriter.NextRecordAsync();

            var products = ProductRepository.ProductsQuery()
                .AsAsyncEnumerable();

            await foreach (var product in products)
            {
                var productCsvDto = Mapper.Map<Product, ProductCsvDto>(product);

                csvWriter.WriteRecord(productCsvDto);
                await csvWriter.NextRecordAsync();

                await writer.FlushAsync();
            }
        }

        public virtual async Task<ProductDtoWithId?> GetProductAsync(int id)
        {
            if (id < 0)
                return default;

            var product = await ProductRepository.GetProductByIdAsync(id);

            if (product == null)
                return default;

            return Mapper.Map<Product, ProductDtoWithId>(product);

        }

        public async Task<ProductsPage<ProductDtoWithId>> GetProductsPageAsync(
            int page, int pageSize)
        {
            var productsPage = new ProductsPage<ProductDtoWithId>();

            productsPage.PageSize = pageSize;
            productsPage.Page = page;
            productsPage.Total = await ProductRepository.ProductsCountAsync();
            productsPage.TotalPages = productsPage.Total / pageSize;

            var products = await ProductRepository.ProductsQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Images)
                .ToListAsync();

            productsPage.Items = Mapper.Map<List<Product>,
                List<ProductDtoWithId>>(products);

            return productsPage;
        }

        public async Task<ProductDto> RemoveProductAsync(int id)
        {
            try
            {
                var product = await GetProductAsync(id);

                if (product == null)
                {
                    throw new Exception("Unexpected error.");
                }

                foreach (var img in product.Images)
                {
                    await ImageStore.DeleteFileAsync(img.FileName);
                }

                await Uow.BeginTransactionAsync();

                await ProductRepository.DeleteProductAsync(id);

                await Uow.SaveChangesAsync();

                await Uow.CommitTransactionAsync();

                return product;
            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ProductDto> GetPhotosByProductIdAsync(int id, Stream targetStream)
        {
            var product = await ProductRepository
                .GetProductByIdAsync(id);

            var names = product.Images.Select(i => i.FileName);

            await using var archive = new ZipArchive(targetStream,
                ZipArchiveMode.Create, true);

            foreach (var imgName in names)
            {
                Stream input = await ImageStore.GetFileAsync(imgName);

                input.Position = 0;

                var entry = archive.CreateEntry(imgName, CompressionLevel.Fastest);

                await using (Stream entryStream = await entry.OpenAsync())
                {
                    await input.CopyToAsync(entryStream);
                }
            }

            return Mapper.Map<Product, ProductDto>(product);
        }

        public async Task UpdateProductAsync(UpdateProductCommand productDto)
        {
            Product product = await ProductRepository
                .GetProductByIdAsync(productDto.Id);

            if (product == null)
                throw new Exception("Unexpected error.");

            try
            {
                await Uow.BeginTransactionAsync();

                product.Title = productDto.Title;
                product.Description = productDto.Description;

                await Uow.SaveChangesAsync();

                await Uow.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await Uow.RollbackTransactionAsync();

                throw;
            }
        }
    }
}
