using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Minio.DataModel.Tags;
using Newtonsoft.Json;
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

        public virtual async Task<ProductDto> CreateFromMultipartAsync(ProductDto productDto,
            IEnumerable<FileContent> content)
        {
            var product = Mapper.Map<ProductDto, Product>(productDto);

            List<ProductImage> images = 
                new List<ProductImage>(content.Count());

            //foreach (var file in content)
            //{
            //    var img = new ProductImage()
            //    {
            //        FileName = file.FileName
            //    }
            //    images.Add()
            //}

            product = await ProductRepository.AddProductAsync(product);

            

            //foreach (FileContent file in content)
            //{
            //    var productImg = new ProductImage()
            //    {
            //        ProductId = product.Id,
            //        FileName = $"{product.Id}_{product.Title}"
            //    };

            //    file.Content.Position = 0;

            //    await ImageStore.PutFileAsync(file.Content, file.FileName);


            //    images.Add(productImg);
            //}

            product.Images = images;

            productDto = Mapper.Map<Product, ProductDto>(product);

            return productDto;
        }

        public virtual async Task<ProductDto> CreateProductAsync(ProductDto productDto)
        {
            var product = Mapper.Map<ProductDto, Product>(productDto);

            try
            {
                await Uow.BeginTransactionAsync();

                product = await ProductRepository.AddProductAsync(product);

                productDto = Mapper.Map<Product, ProductDto>(product);

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

        public virtual async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await ProductRepository.ProductsQuery()
                .ToListAsync();

            var productsDto = Mapper.Map<List<Product>, List<ProductDto>>(products);

            return productsDto;
        }

        public virtual async Task ExportCsvReportAsync(Stream targetStream)
        {
            await using var writer = new StreamWriter(
                targetStream,
                Encoding.UTF8);

            await using var csvWriter = new CsvWriter(writer,
                CultureInfo.InvariantCulture, leaveOpen : true);

            csvWriter.WriteHeader<ProductCsvDto>();

            var products = ProductRepository.ProductsQuery()
                .AsAsyncEnumerable();

            await foreach(var product in products)
            {
                var productCsvDto = Mapper.Map<Product, ProductCsvDto>(product);

                csvWriter.WriteRecord(productCsvDto);
                await csvWriter.NextRecordAsync();

                await writer.FlushAsync();
            }
        }

        public virtual async Task<ProductDto>? GetProductAsync(int id)
        {
            if (id < 0)
                return default;

            var product = await ProductRepository.GetProductByIdAsync(id);

            if (product == null)
                return default;

            return Mapper.Map<Product, ProductDto>(product);

        }

        public async Task<ProductsPage<ProductDto>> GetProductsPageAsync(int page, int pageSize)
        {
            var productsPage = new ProductsPage<ProductDto>();

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
                List<ProductDto>>(products);

            return productsPage;
        }

        public async Task RemoveProductAsync(int id)
        {
            await ProductRepository.DeleteProductAsync(id);
        }

        public async Task GetPhotosByProductIdAsync(int id, Stream targetStream)
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

                using Stream entryStream = entry.Open();

                input.CopyTo(entryStream);

                await entryStream.FlushAsync();
            }
        }
    }
}
