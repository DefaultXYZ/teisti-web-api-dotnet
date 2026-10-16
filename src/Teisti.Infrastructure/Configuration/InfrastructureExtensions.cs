using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teisti.Application.Database;
using Teisti.Infrastructure.Database;

namespace Teisti.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseInMemoryDatabase(ApplicationDbContext.DbName));
        services.AddScoped<IProductsDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        return services;
    }

    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.InitializeAsync();
        }
    }
}
