using Microsoft.EntityFrameworkCore;
using Teisti.Application.Abstractions;
using Teisti.Application.Database;
using Teisti.Domain.Models;

namespace Teisti.Application.Products;

public record GetProductByIdQuery(Guid Id);

public class GetProductByIdQueryHandler(
    IProductsDbContext productsDbContext) : IQueryHandler<GetProductByIdQuery, Product?>
{
    public async Task<Product?> HandleAsync(
        GetProductByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await productsDbContext.Products.AsNoTracking()
            .FirstOrDefaultAsync(
                product => product.Id == query.Id,
                cancellationToken);
    }
}
