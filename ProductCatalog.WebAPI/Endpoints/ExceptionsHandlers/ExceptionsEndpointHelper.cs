namespace ProductCatalog.WebAPI.Endpoints.ExceptionsHandlers;

public static class ExceptionsEndpointHelper
{
    public static async Task HandleProductNotFoundExceptionAsync(
    HttpContext ctx, int? requestedId = null)
    {
        var defaultTitle = "Product not fount.";

        await Results.Problem(
            title: (requestedId.HasValue) ? defaultTitle +
            $"\n id: {requestedId.Value}" :
            defaultTitle,
            statusCode: StatusCodes.Status404NotFound)
            .ExecuteAsync(ctx);
    }

    public static async Task HandleServerErrorException(HttpContext ctx,
        string message = "Unexpected server error")
    {
        await Results.Problem(
            title: message,
            statusCode: StatusCodes.Status503ServiceUnavailable)
            .ExecuteAsync(ctx);
    }

    public static async Task HandleProductAlreadyExsistExceptionAsync(
        HttpContext ctx, string message = "Product already exist")
    {
        await Results.Problem(
            title: message,
            statusCode: StatusCodes.Status400BadRequest)
            .ExecuteAsync(ctx);
    }
}
