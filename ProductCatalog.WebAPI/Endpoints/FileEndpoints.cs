using MediatR;
using Microsoft.AspNetCore.Http.Features;
using ProductCatalog.Application.Queries;
using System.Net.Mime;

namespace ProductCatalog.WebAPI.Endpoints;

public static class FileEndpoints
{
    public static IEndpointRouteBuilder UseFileEndpoints
        (this IEndpointRouteBuilder endpoints, string rootStaticPath)
    {
        var group = endpoints.MapGroup(rootStaticPath);

        group.MapGet("/report", ExportCsvAsync);
        group.MapGet("{productId:int}/photos", GetPhotosByProductId);

        return endpoints;
    }

    public static async Task ExportCsvAsync(
        HttpContext ctx,
        IMediator mediator)
    {
        try
        {
            ctx.Response.ContentType = MediaTypeNames.Text.Csv;
            ctx.Response.Headers.ContentDisposition =
                new ContentDisposition()
                {
                    DispositionType = DispositionTypeNames.Attachment,
                    FileName = $"report.csv",
                    CreationDate = DateTime.UtcNow
                }.ToString();

            var query = new GetProductsCsvReportQuery();

            Stream result = await mediator.Send(query);

            await Results.File(result).ExecuteAsync(ctx);
        }
        catch (Exception ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(ctx);
        }
    }

    public static async Task GetPhotosByProductId(
        int productId,
        HttpContext ctx,
        IMediator mediator)
    {
        try
        {
            if (productId < 0)
            {
                throw new Exception("id < 0.");
            }

            var allowSynchronousIoOption =
                ctx.Features.Get<IHttpBodyControlFeature>();


            ctx.Response.ContentType = MediaTypeNames.Application.Zip;
            ctx.Response.Headers.ContentDisposition =
                new ContentDisposition()
                {
                    FileName = $"photos_{productId}.zip",
                    DispositionType = DispositionTypeNames.Attachment

                }.ToString();

            var query = new GetPhotosByProductIdQuery()
            { Id = productId };
            ;

            var result = await mediator.Send(query);

            await Results.File(result).ExecuteAsync(ctx);

        }
        catch (Exception ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(ctx);
        }
    }
}
