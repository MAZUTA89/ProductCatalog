using Microsoft.AspNetCore.Http.Features;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Core.Interfaces;
using System.Net;
using System.Net.Mime;

namespace ProductCatalog.WebAPI.Endpoints
{
    public static class FileEndpoints
    {
        public static IEndpointRouteBuilder UseFileEndpoints
            (this IEndpointRouteBuilder endpoints, string rootStaticPath)
        {
            var group = endpoints.MapGroup(rootStaticPath);

            group.MapGet("/report", ExportCsvAsync);
            group.MapGet("/photos/{productId:int}", GetPhotosByProductId);

            return endpoints;
        }

        public static async Task<IResult> ExportCsvAsync(
            HttpContext ctx,
            IProductService productService)
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

                await productService.ExportCsvReportAsync(ctx.Response.Body);

                return Results.Empty;
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        public static async Task GetPhotosByProductId(
            int productId,
            HttpContext ctx,
            IProductService productService)
        {
            try
            {
                if(productId < 0)
                {
                    throw new Exception("id < 0.");
                }

                var allowSynchronousIoOption = ctx.Features.Get<IHttpBodyControlFeature>();
                if (allowSynchronousIoOption != null)
                {
                    allowSynchronousIoOption.AllowSynchronousIO = true;
                }

                ctx.Response.Headers.ContentType = MediaTypeNames.Application.Zip;
                ctx.Response.Headers.ContentDisposition =
                    new ContentDisposition()
                    {
                        FileName = $"photos_{productId}.zip",
                        DispositionType = DispositionTypeNames.Attachment

                    }.ToString();

                var productDto = await productService
                    .GetPhotosByProductIdAsync(productId, ctx.Response.Body);
                
            }
            catch(Exception ex)
            {
                ctx.Response.Body.Close();
            }
        }
    }
}
