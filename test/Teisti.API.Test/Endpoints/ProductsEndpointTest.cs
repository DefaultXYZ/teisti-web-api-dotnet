using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Teisti.API.Models;
using Teisti.API.Test.Configuration;
using Teisti.Domain.Models;

namespace Teisti.API.Test.Endpoints;

public sealed class ProductsEndpointTest : IAsyncLifetime
{
    private readonly ApplicationFactory _factory = new();
    private HttpClient _client;

    private static CancellationToken CancellationToken => TestContext.Current.CancellationToken;

    public async ValueTask InitializeAsync()
    {
        await _factory.InitializeAsync();
        var clientOptions = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        };
        _client = _factory.CreateClient(clientOptions);
    }

    public async ValueTask DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task GetAllProducts_OnSuccess_ShouldReturnList()
    {
        // Arrange
        var products = new List<Product>
        {
            new(Guid.NewGuid(), "Test1", 1.2m),
            new(Guid.NewGuid(), "Test2", 2.3m),
            new(Guid.NewGuid(), "Test3", 3.4m)
        };
        await _factory.ExecuteDbAction(async context =>
        {
            await context.Products.AddRangeAsync(products);
        });

        // Act
        var result = await _client.GetFromJsonAsync<List<ProductResponse>>("products/", CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(products.Count, result.Count);
        Assert.Equivalent(
            products.Select(p => (p.Name, p.Price)),
            result.Select(p => (p.Name, p.Price)));
    }

    [Fact]
    public async Task GetAllProducts_AfterPostNew_ShouldReturnList()
    {
        // Arrange
        var postMessage = await _client.PostAsJsonAsync(
            "products/",
            new { name = "Sushi", price = 42.01m },
            CancellationToken);
        Assert.Equal(HttpStatusCode.Created, postMessage.StatusCode);

        // Act
        var result = await _client.GetFromJsonAsync<List<ProductResponse>>(
            "products/",
            CancellationToken);

        // Assert
        Assert.NotNull(result);
        var item = Assert.Single(result);
        Assert.Equal("Sushi", item.Name);
        Assert.Equal(42.01m, item.Price);
    }
}
