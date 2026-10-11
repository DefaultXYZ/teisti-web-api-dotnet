namespace Teisti.API.Configuration;

public static class ApiExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        return services;
    }

    public static void SetupSwagger(this WebApplication app)
    {
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Teisti API v1"));
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            foreach (var url in app.Urls)
            {
                app.Logger.LogInformation("Swagger UI: {url}/swagger", url);
            }
        });
    }
}
