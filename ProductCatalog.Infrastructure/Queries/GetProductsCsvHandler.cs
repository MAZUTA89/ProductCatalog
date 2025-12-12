
using AutoMapper;
using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using ProductCatalog.Infrastructure.Repositories.Abstructions;
using System.Globalization;
using System.Text;

namespace ProductCatalog.Infrastructure.Queries;

public class GetProductsCsvHandler
    : IRequestHandler<GetProductsCsvReportQuery, Stream>
{
    protected IProductRepository ProductRepository { get; set; }
    protected IMapper Mapper { get; set; }

    public GetProductsCsvHandler(IProductRepository productRepository)
    {
        ProductRepository = productRepository;
    }

    public async Task<Stream> Handle(GetProductsCsvReportQuery request,
        CancellationToken ct)
    {
        Stream result = new MemoryStream();

        await using var writer = new StreamWriter(
            result,
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

        return result;
    }
}
