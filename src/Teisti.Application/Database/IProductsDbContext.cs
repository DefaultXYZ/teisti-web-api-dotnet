using Microsoft.EntityFrameworkCore;
using Teisti.Domain.Models;

namespace Teisti.Application.Database;

public interface IProductsDbContext : IEntityDbContext
{
    DbSet<Product> Products { get; }
}
