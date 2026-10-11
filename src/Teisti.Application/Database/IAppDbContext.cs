using Teisti.Domain.Models;

namespace Teisti.Application.Database;

public interface IAppDbContext
{
    Task<List<Product>> LoadProductsAsync();
    Task<Product> AddNewProductAsync(string name, decimal price);
}
