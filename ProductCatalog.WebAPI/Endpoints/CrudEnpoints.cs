using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;

namespace ProductCatalog.WebAPI.Endpoints
{
    public static class CrudEnpoints
    {
        public static IEndpointRouteBuilder UseCRUDEndpoints
            (this IEndpointRouteBuilder endpoints, string rootStaticPath)
        {
            var group = endpoints.MapGroup(rootStaticPath);

            group.MapGet("/{id:int}", GetProductById);

            group.MapGet("/all", GetAllProductsAsync);

            group.MapPost("", CreateProductMultipart)
                .DisableAntiforgery();

            group.MapPut("", UpdateProductAsync);

            group.MapDelete("/{id:int}", RemoveProduct);
            return endpoints;
        }

        public static async Task<IResult> GetProductById(
            int id,
            IProductService productService)
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

        public static async Task GetAllProductsAsync(
            HttpContext ctx,
            IProductService productService)
        {
            try
            {
                var product = await productService.GetAllProductsAsync();

                await Results.Json(product).ExecuteAsync(ctx);
            }
            catch(Exception ex)
            {
                await Results.BadRequest(ex.Message).ExecuteAsync(ctx);
            }
        }

        public static async Task<IResult> CreateProductMultipart(
            [FromForm] CreateProductCommand productDto, 
            [FromForm] IFormFileCollection files,
            IProductService productService)
        {
            try
            {
                IEnumerable<FileContent> content = files.Select(f => new FileContent()
                {
                    FileName = f.FileName,
                    Content = f.OpenReadStream()
                });

                var createdProductDto = await productService
                    .CreateFromMultipartAsync(productDto, content);

                return Results.Ok(createdProductDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        public static async Task<IResult> RemoveProduct(
            int id,
            IProductService productService)
        {
            try
            {
                if (id < 0)
                    return Results.Json(new { message = "id < 0" });

                var productsDto = await productService.RemoveProductAsync(id);

                return Results.Ok($"Product removed with Title: {productsDto.Title}.");
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
}
