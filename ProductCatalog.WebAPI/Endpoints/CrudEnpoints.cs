using ProductCatalog.Application.Interfaces;

namespace ProductCatalog.WebAPI.Endpoints
{
    public static class CrudEnpoints
    {
        public static IEndpointRouteBuilder SetUpCRUDEndpoints
            (this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/products");

            group.MapGet("", GetProductById);

            //group.MapPost("{id}:int", );

            //group.MapDelete("{id}:int", );

            //group.MapPut("{id}:int", );

            return endpoints;
        }

        public static async Task<IResult> GetProductById(
            int id,
            IProductService productService
            )
        {
            try
            {
                var product = await productService.GetProductAsync(id);

                if (product == null)
                {
                    return Results.Json(new { message = "Unexpected error." }, statusCode: 500);
                }
                else
                {
                    return Results.Json(product);
                }

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
