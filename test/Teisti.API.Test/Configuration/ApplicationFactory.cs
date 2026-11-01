using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Teisti.Infrastructure.Database;

namespace Teisti.API.Test.Configuration;

public class ApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var existingDbDescriptor = services.Single(desc =>
                desc.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>));
            services.Remove(existingDbDescriptor);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("db_teisti"));
        });
    }
}
