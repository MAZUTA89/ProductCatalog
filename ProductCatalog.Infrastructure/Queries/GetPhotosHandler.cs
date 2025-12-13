using AutoMapper;
using MediatR;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Core.Entities;
using ProductCatalog.Domain.Core.Interfaces;
using System.IO.Compression;

namespace ProductCatalog.Infrastructure.Queries;

public class GetPhotosHandler
    : IRequestHandler<GetPhotosByProductIdQuery, Stream>
{

    protected IProductRepository ProductRepository { get; set; }
    protected IImageStorage ImageStorage { get; set; }

    public GetPhotosHandler(IProductRepository productRepository,
        IImageStorage imageStorage)
    {
        ProductRepository = productRepository;
        ImageStorage = imageStorage;
    }

    public async Task<Stream> Handle(GetPhotosByProductIdQuery request,
        CancellationToken ct)
    {
        Stream result = new MemoryStream();

        var product = await ProductRepository
            .GetProductByIdAsync(request.Id);

        var names = product.Images.Select(i => i.FileName);

        using (var archive = new ZipArchive(result,
            ZipArchiveMode.Create, true))
        {
            foreach (var imgName in names)
            {
                Stream input = await ImageStorage.GetFileAsync(imgName);

                input.Position = 0;

                var entry = archive.CreateEntry(imgName,
                    CompressionLevel.Fastest);

                await using (Stream entryStream = await entry.OpenAsync())
                {
                    await input.CopyToAsync(entryStream);
                }
            }
        }

        result.Position = 0;

        return result;
    }
}
