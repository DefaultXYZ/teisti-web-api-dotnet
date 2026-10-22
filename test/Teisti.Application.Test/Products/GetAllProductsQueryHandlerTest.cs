using MockQueryable.NSubstitute;
using NSubstitute;
using Teisti.Application.Database;
using Teisti.Application.Products;
using Teisti.Domain.Models;

namespace Teisti.Application.Test.Products;

[TestClass]
public class GetAllProductsQueryHandlerTest
{
    public TestContext TestContext { get; set; }

    private IProductsDbContext _productsDbContext = null!;
    private GetAllProductsQueryHandler _handler = null!;

    [TestInitialize]
    public void TestInit()
    {
        _productsDbContext = Substitute.For<IProductsDbContext>();
        _handler = new GetAllProductsQueryHandler(_productsDbContext);
    }

    [TestMethod]
    public async Task HandleAsync_HasProducts_ShouldReturnAll()
    {
        // Arrange
        var expected = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = expected.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        var result = await _handler.HandleAsync(new GetAllProductsQuery(), TestContext.CancellationToken);

        // Assert
        Assert.AreSequenceEqual(expected, result);
    }

    [TestMethod]
    public async Task HandleAsync_NoProducts_ShouldReturnEmpty()
    {
        // Arrange
        var expected = new List<Product>();
        var mockedProductDbSet = expected.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        var result = await _handler.HandleAsync(new GetAllProductsQuery(), TestContext.CancellationToken);

        // Assert
        Assert.IsEmpty(result);
    }
}
