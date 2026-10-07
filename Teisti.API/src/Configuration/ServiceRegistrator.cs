namespace Teisti.API;

public static class ServiceRegistrator
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddSingleton<FoodEndpoint>();
        return services;
    } 
}
