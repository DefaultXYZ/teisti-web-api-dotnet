using Microsoft.AspNetCore.Mvc.Testing;

namespace Teisti.API.Test.Configuration;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
            {
                // Insert test database binding here
            }
        );
    }
}
