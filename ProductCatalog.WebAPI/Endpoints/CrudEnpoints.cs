using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Queries;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Infrastructure.Repositories.NpgRepository.Exceptions;

namespace ProductCatalog.WebAPI.Endpoints;

public static class CrudEnpoints
{
    public static IEndpointRouteBuilder UseCRUDEndpoints
        (this IEndpointRouteBuilder endpoints, string rootStaticPath)
    {
        var group = endpoints.MapGroup(rootStaticPath);

        group.MapGet("/{id:int}", GetProductById)
            .AddOpenApiOperationTransformer((operation, ctx, ct) =>
            {
                return Task.CompletedTask;
            });

        group.MapGet("/all", GetAllProductsAsync);

        group.MapPost("", CreateProductMultipart)
            .DisableAntiforgery();

        group.MapPut("", UpdateProductAsync);

        group.MapDelete("/{id:int}", RemoveProduct);
        return endpoints;
    }
    /// <summary>
    /// Get a product by product id.
    /// </summary>
    /// <returns>Product item</returns>
    /// <remarks>
    /// Sample request:
    /// GET api/products/5
    /// </remarks>
    public static async Task<IResult> GetProductById(
        int id,
        IMediator mediator)
    {
        try
        {
            var query = new GetProductByIdQuery() { Id = id };

            var product = await mediator.Send(query);

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

    public static async Task GetAllProductsAsync(
        HttpContext ctx,
        IMediator mediator
        )
    {
        try
        {
            var query = new GetProductsQuery();

            var product = await mediator.Send(query);

            await Results.Json(product).ExecuteAsync(ctx);
        }
        catch(Exception ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(ctx);
        }
    }

    public static async Task<IResult> CreateProductMultipart(
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

            return Results.Json(createdProductDto);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> RemoveProduct(
        int id,
        IMediator mediator)
    {
        try
        {
            var command = new DeleteProductCommand()
            { Id = id };
            

            if (command.Id < 0)
                return Results.Json(new { message = "id < 0" });

            var result = await mediator.Send(command);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateProductAsync(
        [FromForm] UpdateProductCommand updateProductCommand,
        IProductService productService)
    {
        try
        {
            await productService.UpdateProductAsync(updateProductCommand);

            return Results.Ok();
        }
        catch(Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
