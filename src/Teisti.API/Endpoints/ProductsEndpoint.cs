using Microsoft.AspNetCore.Http.HttpResults;
using Teisti.API.Extensions;
using Teisti.API.Models;
using Teisti.Application.Products;

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
        GetAllProductsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var products = await handler.HandleAsync(new GetAllProductsQuery(), cancellationToken);
        var productResponses = products.Select(product => product.ToResponse()).ToList();
        return TypedResults.Ok(productResponses);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound>> GetProductById(
        Guid id,
        GetProductByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var product = await handler.HandleAsync(new GetProductByIdQuery(id), cancellationToken);
        return product is null ? TypedResults.NotFound() : TypedResults.Ok(product.ToResponse());
    }

    private static async Task<CreatedAtRoute<ProductResponse>> PostNewProduct(
        CreateProductRequest request,
        CreateProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var insertedProduct = await handler.HandleAsync(
            new CreateProductCommand(request.Name, request.Price),
            cancellationToken);
        return TypedResults.CreatedAtRoute(
            insertedProduct.ToResponse(),
            nameof(GetProductById),
            new { id = insertedProduct.Id });
    }
}
