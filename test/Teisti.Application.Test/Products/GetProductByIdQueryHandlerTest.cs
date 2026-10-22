using MockQueryable.NSubstitute;
using NSubstitute;
using Teisti.Application.Database;
using Teisti.Application.Products;
using Teisti.Domain.Models;

namespace Teisti.Application.Test.Products;

[TestClass]
public class GetProductByIdQueryHandlerTest
{
    public TestContext TestContext { get; set; }

    private IProductsDbContext _productsDbContext = null!;
    private GetProductByIdQueryHandler _handler = null!;

    [TestInitialize]
    public void TestInit()
    {
        _productsDbContext = Substitute.For<IProductsDbContext>();
        _handler = new GetProductByIdQueryHandler(_productsDbContext);
    }

    [TestMethod]
    public async Task HandleAsync_HasProduct_ShouldReturn()
    {
        // Arrange
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = products.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);
        var expected = products[0];

        // Act
        var result = await _handler.HandleAsync(
            new GetProductByIdQuery(expected.Id),
            TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public async Task HandleAsync_NoProduct_ShouldReturnNull()
    {
        // Arrange
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = products.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        var result = await _handler.HandleAsync(
            new GetProductByIdQuery(Guid.NewGuid()),
            TestContext.CancellationToken);

        // Assert
        Assert.IsNull(result);
    }
}
