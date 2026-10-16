using Microsoft.EntityFrameworkCore;
using Teisti.Application.Abstractions;
using Teisti.Application.Database;
using Teisti.Domain.Models;

namespace Teisti.Application.Products;

public class GetAllProductsQuery;

public class GetAllProductsQueryHandler(IProductsDbContext productsDbContext)
    : IQueryHandler<GetAllProductsQuery, List<Product>>
{
    public async Task<List<Product>> HandleAsync(
        GetAllProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await productsDbContext.Products.AsNoTracking().ToListAsync(cancellationToken);
    }
}
