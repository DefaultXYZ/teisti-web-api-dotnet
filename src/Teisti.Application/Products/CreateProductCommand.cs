using Teisti.Application.Abstractions;
using Teisti.Application.Database;
using Teisti.Domain.Models;

namespace Teisti.Application.Products;

public record CreateProductCommand(string Name, decimal Price);

public class CreateProductCommandHandler(
    IProductsDbContext productsDbContext) : ICommandHandler<CreateProductCommand, Product>
{
    public async Task<Product> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        var product = await productsDbContext.Products.AddAsync(
            Product.Create(command.Name, command.Price),
            cancellationToken);
        await productsDbContext.ApplyChangesAsync(cancellationToken);
        return product.Entity;
    }
}
