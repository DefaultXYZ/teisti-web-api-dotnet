using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Teisti.API;

public class FoodEndpoint
{
    private readonly List<string> _foodList = ["Pizza", "Hamburger", "Chicken"];

    public static void UseFoodEndpoint(IEndpointRouteBuilder app)
    {
        var foodEndpoint = app.ServiceProvider.GetRequiredService<FoodEndpoint>();
        var foodGroup = app.MapGroup("/Food");
        foodGroup.MapGet("/", foodEndpoint.GetAllFood);
        foodGroup.MapPost("/", foodEndpoint.PostNewFood);
    }

    private async Task<Ok<List<string>>> GetAllFood()
    {
        await Task.Delay(500);
        return TypedResults.Ok(_foodList);
    }

    private async Task<Ok<string>> PostNewFood([FromBody] NewFoodRequest request)
    {
        _foodList.Add(request.Name);
        await Task.Delay(500);
        return TypedResults.Ok(request.Name);
    }
}

public class NewFoodRequest
{
    public string Name { get; set; } = "";
}
