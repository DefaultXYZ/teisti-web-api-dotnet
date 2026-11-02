using Teisti.API.Configuration;
using Teisti.API.Endpoints;
using Teisti.Application.Configuration;
using Teisti.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Local, git-ignored, environment settings
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true);

builder.Services.AddOpenApi()
    .AddValidation()
    .AddInfrastructure(builder.Configuration)
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
