using Teisti.API.Configuration;
using Teisti.API.Endpoints;
using Teisti.Application.Configuration;
using Teisti.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.SetupSwagger();
}

app.UseHttpsRedirection();
app.MapProductsEndpoint();
app.Run();
