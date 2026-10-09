using Teisti.API.Configuration;
using Teisti.API.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
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
