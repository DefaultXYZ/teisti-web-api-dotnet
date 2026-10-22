using MockQueryable.NSubstitute;
using NSubstitute;
using Teisti.Application.Database;
using Teisti.Application.Products;
using Teisti.Domain.Models;
using Teisti.Domain.Providers;

namespace Teisti.Application.Test.Products;

[TestClass]
public class CreateProductCommandHandlerTest
{
    public TestContext TestContext { get; set; }

    private IProductsDbContext _productsDbContext = null!;
    private IIdProvider _idProvider = null!;
    private CreateProductCommandHandler _handler = null!;

    [TestInitialize]
    public void TestInit()
    {
        _productsDbContext = Substitute.For<IProductsDbContext>();
        _idProvider = Substitute.For<IIdProvider>();
        _handler = new CreateProductCommandHandler(_productsDbContext, _idProvider);
    }

    [TestMethod]
    public async Task HandleAsync_IsValid_ShouldInvokeAddAsync()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _idProvider.NewId().Returns(expectedId);
        var expected = new Product(expectedId, "Expected", 42);
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = products.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        await _handler.HandleAsync(
            new CreateProductCommand(expected.Name, expected.Price),
            TestContext.CancellationToken);

        // Assert
        await _productsDbContext.Products.Received()
            .AddAsync(
                Arg.Is<Product>(product => product.Id == expectedId &&
                                           product.Name == expected.Name &&
                                           product.Price == expected.Price),
                Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task HandleAsync_IsValid_ShouldInvokeApplyChanges()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _idProvider.NewId().Returns(expectedId);
        var expected = new Product(expectedId, "Expected", 42);
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = products.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        await _handler.HandleAsync(
            new CreateProductCommand(expected.Name, expected.Price),
            TestContext.CancellationToken);

        // Assert
        await _productsDbContext.Received().ApplyChangesAsync(TestContext.CancellationToken);
    }

    [TestMethod]
    public async Task HandleAsync_IsValid_ShouldReturnNewProduct()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        _idProvider.NewId().Returns(expectedId);
        var expected = new Product(expectedId, "Expected", 42);
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 4.5m)
        };
        var mockedProductDbSet = products.BuildMockDbSet();
        _productsDbContext.Products.Returns(mockedProductDbSet);

        // Act
        var result = await _handler.HandleAsync(
            new CreateProductCommand(expected.Name, expected.Price),
            TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(expected.Id, result.Id);
        Assert.AreEqual(expected.Name, result.Name);
        Assert.AreEqual(expected.Price, result.Price);
    }
}
