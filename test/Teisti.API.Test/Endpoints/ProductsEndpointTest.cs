using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Teisti.API.Models;
using Teisti.API.Test.Configuration;

namespace Teisti.API.Test.Endpoints;

[TestClass]
public class ProductsEndpointTest
{
    private ApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new ApplicationFactory();
        _client = _factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task GetAllProducts_OnApplicationInit_ShouldReturnTwo()
    {
        // Act
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>(
            "products/",
            TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(products);
        Assert.HasCount(2, products);
    }

    [TestMethod]
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
            TestContext.CancellationToken);
        Assert.AreEqual(HttpStatusCode.Created, postMessage.StatusCode);

        // Act
        var products = await _client.GetFromJsonAsync<List<ProductResponse>>(
            "products/",
            TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(products);
        Assert.HasCount(3, products);
    }
}
