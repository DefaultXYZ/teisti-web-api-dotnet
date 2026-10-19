using Teisti.Application.Abstractions;
using Teisti.Application.Database;
using Teisti.Domain.Models;
using Teisti.Domain.Providers;

namespace Teisti.Application.Products;

public record CreateProductCommand(string Name, decimal Price);

public class CreateProductCommandHandler(
    IProductsDbContext productsDbContext,
    IIdProvider idProvider) : ICommandHandler<CreateProductCommand, Product>
{
    public async Task<Product> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        var product = new Product(idProvider.NewId(), command.Name, command.Price);
        var createdProduct = await productsDbContext.Products.AddAsync(product, cancellationToken);
        await productsDbContext.ApplyChangesAsync(cancellationToken);
        return createdProduct.Entity;
    }
}
