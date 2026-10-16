using Microsoft.Extensions.DependencyInjection;
using Teisti.Application.Products;

namespace Teisti.Application.Configuration;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetAllProductsQueryHandler>();
        services.AddScoped<GetProductByIdQueryHandler>();
        services.AddScoped<CreateProductCommandHandler>();
        return services;
    }
}
