namespace Teisti.Domain.Models;

public class Product
{
    public const string EmptyName = "";

    public Guid Id { get; private set; }
    public string Name { get; private set; } = EmptyName;
    public decimal Price { get; private set; }

    public static Product Create(string name, decimal price)
    {
        return new Product
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Price = price
        };
    }
}
