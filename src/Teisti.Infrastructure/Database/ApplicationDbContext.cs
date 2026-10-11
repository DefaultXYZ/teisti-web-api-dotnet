using Teisti.Application.Database;
using Teisti.Domain.Models;

namespace Teisti.Infrastructure.Database;

public class ApplicationDbContext : IAppDbContext
{
    private readonly List<Product> _products =
    [
        Product.Create("Pizza", 12.4m),
        Product.Create("Burger", 6.5m)
    ];

    public async Task<List<Product>> LoadProductsAsync()
    {
        return _products;
    }

    public async Task<Product> AddNewProductAsync(string name, decimal price)
    {
        var newProduct = Product.Create(name, price);
        _products.Add(newProduct);
        return newProduct;
    }
}
