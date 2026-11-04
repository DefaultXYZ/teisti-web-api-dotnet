using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Teisti.Domain.Models;
using Teisti.Infrastructure.Database;
using Testcontainers.PostgreSql;

namespace Teisti.API.Test.Configuration;

public sealed class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:18.6-alpine3.24")
            .WithDatabase("db_teisti")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    private string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(ConnectionString));
        });
    }

    public DbSet<Product> GetProductSet()
    {
        return Services.GetRequiredService<ApplicationDbContext>().Products;
    }

    internal async Task ExecuteDbAction(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action.Invoke(dbContext);
        await dbContext.SaveChangesAsync();
    }
}
