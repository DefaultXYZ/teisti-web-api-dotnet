using Microsoft.Extensions.DependencyInjection;
using Teisti.Application.Database;
using Teisti.Infrastructure.Database;

namespace Teisti.Infrastructure.Configuration;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAppDbContext, ApplicationDbContext>();
        return services;
    }
}
