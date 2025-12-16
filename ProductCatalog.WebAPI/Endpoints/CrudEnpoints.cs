using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Queries;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.WebAPI.Endpoints.ExceptionsHandlers;
using ProductCatalog.Infrastructure.Repositories.Exceptions;

namespace ProductCatalog.WebAPI.Endpoints;

public static class CrudEnpoints
{
    public static IEndpointRouteBuilder UseCRUDEndpoints
        (this IEndpointRouteBuilder endpoints, string rootStaticPath)
    {
        var group = endpoints.MapGroup(rootStaticPath);

        group.MapGet("/{id:int}", GetProductById)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Get a product by product id.";
                operation.Description = """
                **Sample request**

                `GET /api/products/5`

                **Sample response**

                ```json
                {
                  "productId": "5",
                  "title": "product title",
                  "description": "product description",
                  "images": [
                    {
                      "fileName": "file name"
                    }
                  ]
                }
                ```
                """;
                operation.Responses["400"].Description = "Product not found";
                operation.Responses["503"].Description = "Unexpected error";

                return operation;
            })
            .Produces(StatusCodes.Status200OK,
            responseType: typeof(ProductDtoWithId))
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status503ServiceUnavailable);

        group.MapGet("/all", GetAllProductsAsync)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Returns all products.";
                operation.Description = """
                **Sample request**

                `GET api/products/all`
                """;

                operation.Responses["200"].Description = "Return all products";
                operation.Responses["503"].Description = "Unexpected error";
                return operation;
            })
            .Produces(StatusCodes.Status200OK, responseType: typeof(IEnumerable<ProductDtoWithId>))
            .Produces(StatusCodes.Status503ServiceUnavailable);

        group.MapPost("", CreateProductMultipart)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Adds a new product.";
                operation.Description = """
                **Sample request**

                `POST /api/products`
                
                *Multipart query parameners:*

                `"title" : "product title"`;

                `"description" : "product description"`;

                `"files" : images content`.
                """;

                operation.Responses["400"].Description = "Product exist.";
                operation.Responses["503"].Description = "Unexpected error";

                return operation;
            })
            .Produces(StatusCodes.Status200OK, responseType: typeof(ProductDtoWithId))
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .DisableAntiforgery();
            

        group.MapPut("", UpdateProductAsync);

        group.MapDelete("/{id:int}", RemoveProduct);
        return endpoints;
    }

    public static async Task GetProductById(
        int id,
        IMediator mediator,
        HttpContext ctx)
    {
        try
        {
            var query = new GetProductByIdQuery() { Id = id };

            var product = await mediator.Send(query);

            await Results.Json(product, statusCode: StatusCodes.Status200OK)
                .ExecuteAsync(ctx);
        }
        catch (ProductNotFoundException ex)
        {
            await ExceptionsEndpointHelper.
                HandleProductNotFoundExceptionAsync(ctx, id);
        }
        catch (Exception ex)
        {
            await ExceptionsEndpointHelper
                .HandleServerErrorException(ctx);
        }
    }

    public static async Task GetAllProductsAsync(
        HttpContext ctx,
        IMediator mediator
        )
    {
        try
        {
            var query = new GetProductsQuery();

            var product = await mediator.Send(query);

            await Results.Json(product, statusCode: StatusCodes.Status200OK)
                .ExecuteAsync(ctx);
        }
        catch (Exception ex)
        {
            await ExceptionsEndpointHelper
                .HandleServerErrorException(ctx);
        }
    }

    public static async Task CreateProductMultipart(
        [FromForm] string title,
        [FromForm] string description,
        [FromForm] IFormFileCollection files,
        HttpContext ctx,
        IMediator mediator)
    {
        try
        {
            var command = new CreateProductWithImagesCommand()
            {
                Title = title,
                Description = description,
            };

            IEnumerable<FileContent> content = files.Select(
                f => new FileContent()
                {
                    FileName = f.FileName,
                    Content = f.OpenReadStream()
                });

            command.Files = content;


            var createdProductDto = await mediator.Send(command);

            await Results.Json(createdProductDto,
                statusCode: StatusCodes.Status201Created)
                .ExecuteAsync(ctx);
        }
        catch (ProductAlreadyExistException ex)
        {
            await ExceptionsEndpointHelper
                .HandleProductAlreadyExsistExceptionAsync(ctx);
        }
        catch (Exception ex)
        {
            await ExceptionsEndpointHelper
                .HandleServerErrorException(ctx);
        }
    }

    public static async Task RemoveProduct(
        int id,
        IMediator mediator,
        HttpContext ctx)
    {
        try
        {
            var command = new DeleteProductCommand()
            { Id = id };

            var result = await mediator.Send(command);

            Results.Ok(result);
        }
        catch (ProductNotFoundException ex)
        {
            await ExceptionsEndpointHelper
                .HandleProductNotFoundExceptionAsync(ctx);
        }
        catch (Exception ex)
        {
            await ExceptionsEndpointHelper
                .HandleServerErrorException(ctx);
        }
    }

    public static async Task UpdateProductAsync(
        [FromForm] UpdateProductCommand updateProductCommand,
        IProductService productService,
        HttpContext ctx)
    {
        await ExceptionsEndpointHelper.HandleServerErrorException(ctx);

        try
        {
            await productService.UpdateProductAsync(updateProductCommand);

            await Results.Ok()
                .ExecuteAsync(ctx);
        }
        catch (Exception ex)
        {
            await Results.BadRequest(ex.Message)
                .ExecuteAsync(ctx);
        }
    }
}
