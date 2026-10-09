namespace Teisti.API;

public class AppDb
{
    private readonly List<Product> _products =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Pizza",
            Price = 12.4m
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Burger",
            Price = 6.5m
        }
    ];

    public IReadOnlyList<Product> Products => _products;

    public Product AddNewProduct(string name, decimal price)
    {
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };
        _products.Add(newProduct);
        return newProduct;
    }
}

public class Product
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
}
