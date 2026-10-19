using Microsoft.EntityFrameworkCore;
using Teisti.Application.Database;
using Teisti.Domain.Models;
using Teisti.Domain.Providers;

namespace Teisti.Infrastructure.Database;

internal class ApplicationDbContext(
    DbContextOptions options,
    IIdProvider idProvider) : DbContext(options), IProductsDbContext
{
    public const string DbName = "db_teisti";

    public DbSet<Product> Products => Set<Product>();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        // TODO Remove this once real database is setup
        await Products.AddRangeAsync(
            [
                new Product(idProvider.NewId(), "Pizza", 12.4m),
                new Product(idProvider.NewId(), "Burger", 6.5m)
            ],
            cancellationToken: cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task ApplyChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
