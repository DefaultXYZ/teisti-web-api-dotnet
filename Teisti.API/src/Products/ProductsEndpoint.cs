using Microsoft.AspNetCore.Http.HttpResults;

namespace Teisti.API;

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

    private static async Task<Ok<List<ProductResponse>>> GetProducts(AppDb db, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var productResponses = db.Products.Select(product => product.ToResponse()).ToList();
        return TypedResults.Ok(productResponses);
    }

    private static async Task<Results<Ok<ProductResponse>, NotFound>> GetProductById(
        Guid id,
        AppDb db,
        CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var product = db.Products.FirstOrDefault(product => product.Id == id);
        return product is null ? TypedResults.NotFound() : TypedResults.Ok(product.ToResponse());
    }

    private static async Task<CreatedAtRoute<ProductResponse>> PostNewProduct(
        CreateProductRequest request,
        AppDb db,
        CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        var insertedProduct = db.AddNewProduct(request.Name, request.Price);
        return TypedResults.CreatedAtRoute(
            insertedProduct.ToResponse(),
            nameof(GetProductById),
            new { id = insertedProduct.Id });
    }
}

public record ProductResponse(Guid Id, string Name, decimal Price);

public record CreateProductRequest(string Name, decimal Price);

public static class ProductExtensions
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(product.Id, product.Name, product.Price);
    }
}
