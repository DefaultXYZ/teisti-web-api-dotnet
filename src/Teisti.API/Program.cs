using Teisti.API.Configuration;
using Teisti.API.Endpoints;
using Teisti.Application.Configuration;
using Teisti.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi()
    .AddInfrastructure()
    .AddApplication()
    .AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeInfrastructureAsync();

    app.MapOpenApi();
    app.SetupSwagger();
}

app.UseHttpsRedirection();
app.MapProductsEndpoint();
app.Run();
