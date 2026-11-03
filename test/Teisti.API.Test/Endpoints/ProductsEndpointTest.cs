using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Teisti.API.Models;
using Teisti.API.Test.Configuration;

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
    public async Task GetAllProducts_OnApplicationInit_ShouldReturnTwo()
    {
        // Act
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>("products/", CancellationToken);

        // Assert
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task GetAllProducts_AfterPostNew_ShouldReturnThree()
    {
        // Arrange
        const string requestJson = """
                                   {
                                     "name": "Sushi",
                                     "price": 42.01
                                   }
                                   """;
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var postMessage = await _client.PostAsync(
            "products/",
            requestContent,
            CancellationToken);
        Assert.Equal(HttpStatusCode.Created, postMessage.StatusCode);

        // Act
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>(
            "products/",
            CancellationToken);

        // Assert
        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
    }
}
