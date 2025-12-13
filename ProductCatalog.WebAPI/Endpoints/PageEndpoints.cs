using MediatR;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Queries;
using System.Reflection.Metadata;

namespace ProductCatalog.WebAPI.Endpoints
{
    public static class PageEndpoints
    {
        public static IEndpointRouteBuilder UsePageEndpoints
            (this IEndpointRouteBuilder endpoints,
            string rootPrefix)
        {
            var group = endpoints.MapGroup(rootPrefix);

            group.MapGet("/page", HandlePagedRequest);

            return endpoints;
        }

        public static async Task HandlePagedRequest(
            int page,
            int pageSize,
            HttpContext ctx,
            IMediator mediator)
        {
            try
            {
                var query = new GetProductsPageQuery()
                { Page = page, PageSize = pageSize };

                var pageDto = await mediator.Send(query);

                await Results.Json(pageDto).ExecuteAsync(ctx);
            }
            catch (Exception ex)
            {
                await Results.BadRequest(ex.Message).ExecuteAsync(ctx);
            }
        }
    }
}
