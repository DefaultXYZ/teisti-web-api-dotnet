using Microsoft.EntityFrameworkCore;
using Teisti.Application.Database;
using Teisti.Domain.Models;
using Teisti.Infrastructure.Database.Configurations;

namespace Teisti.Infrastructure.Database;

internal class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options), IProductsDbContext
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new ProductEntityConfiguration().Configure(modelBuilder.Entity<Product>());
    }

    public async Task ApplyChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
