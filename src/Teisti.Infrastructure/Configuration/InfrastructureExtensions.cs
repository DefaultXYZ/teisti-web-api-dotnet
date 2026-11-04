using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Teisti.Application.Database;
using Teisti.Domain.Providers;
using Teisti.Infrastructure.Database;
using Teisti.Infrastructure.Providers;

namespace Teisti.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("AppDatabase") ??
                               throw new InvalidOperationException("Missing connection string from settings file");
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Username = configurationManager["Db:User"],
            Password = configurationManager["Db:Password"]
        };
        services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseNpgsql(connectionStringBuilder.ConnectionString)
                .UseSnakeCaseNamingConvention());
        services.AddScoped<IIdProvider, IdProvider>();
        services.AddScoped<IProductsDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        return services;
    }

    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Avoid migration for integration tests
        if (!dbContext.Database.IsInMemory())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
