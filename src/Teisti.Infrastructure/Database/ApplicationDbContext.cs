using Microsoft.EntityFrameworkCore;
using Teisti.Application.Database;
using Teisti.Domain.Models;

namespace Teisti.Infrastructure.Database;

internal class ApplicationDbContext(
    DbContextOptions options) : DbContext(options), IProductsDbContext
{
    public const string DbName = "db_teisti";

    public DbSet<Product> Products => Set<Product>();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        // TODO Remove this once real database is setup
        await Products.AddRangeAsync(
            [
                Product.Create("Pizza", 12.4m),
                Product.Create("Burger", 6.5m)
            ],
            cancellationToken: cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task ApplyChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
