using Microsoft.Extensions.DependencyInjection;

namespace Teisti.Application.Configuration;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
