using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Core.Interfaces;
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
            group.MapGet("/photos", GetPhotosByProductId);

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

        public static async Task<IResult> GetPhotosByProductId(
            int id,
            HttpContext ctx,
            IProductService productService)
        {
            try
            {
                if(id < 0)
                {
                    throw new Exception("id < 0.");
                }

                await productService
                    .GetPhotosByProductIdAsync(id, ctx.Response.Body);

                return Results.Empty;
            }
            catch(Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
