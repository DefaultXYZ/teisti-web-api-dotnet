using Microsoft.AspNetCore.Http.HttpResults;
using Teisti.API.Extensions;
using Teisti.API.Models;
using Teisti.Application.Database;

namespace Teisti.API.Endpoints;

public static class ProductsEndpoint
{
    public static void MapProductsEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");
        group.MapGet("/", GetProducts)
            .WithName(nameof(GetProducts));
        group.MapGet("/{id:guid}", GetProductById)
            .WithName(nameof(GetProductById));
        group.MapPost("/", PostNewProduct)
            .WithName(nameof(PostNewProduct));
    }

    private static async Task<Ok<List<ProductResponse>>> GetProducts(
        IAppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var products = await dbContext.LoadProductsAsync();
        var productResponses = products.Select(product => product.ToResponse()).ToList();
        return TypedResults.Ok(productResponses);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound>> GetProductById(
        Guid id,
        IAppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var products = await dbContext.LoadProductsAsync();
        var product = products.FirstOrDefault(product => product.Id == id);
        return product is null ? TypedResults.NotFound() : TypedResults.Ok(product.ToResponse());
    }

    private static async Task<CreatedAtRoute<ProductResponse>> PostNewProduct(
        CreateProductRequest request,
        IAppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var insertedProduct = await dbContext.AddNewProductAsync(request.Name, request.Price);
        return TypedResults.CreatedAtRoute(
            insertedProduct.ToResponse(),
            nameof(GetProductById),
            new { id = insertedProduct.Id });
    }
}
