using Carter;
using foodswap.DTOs.FoodDTOs;
using foodswap.Filters;
using Serilog;

namespace foodswap.Endpoints;
public class FoodEndpoints : BaseEndpoint
{
    public FoodEndpoints()
        :base("/foods")
    {
        WithTags("Foods");
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", (IConfiguration configuration) =>
        {
            return Ok(
                new List<FoodResponse>()
                {
                    new FoodResponse()
                    {
                        Id = Guid.NewGuid(),
                        Name = "apple",
                        Portion = 100,
                        Calories = 52,
                        Carbohydrates = 0.1m,
                        Protein = 0.2m,
                        Fat = 0.3m,
                        Type = "VEGETABLE"
                    }
                }, "Foods retrieved successfully");
        })
        .RequireAuthorization(p => p.RequireRole("user", "admin"));

        app.MapGet("/{id}", (Guid id) =>
        {

            return Ok(
                new FoodResponse() 
                {
                    Id = id,
                    Name = "apple",
                    Portion = 100,
                    Calories = 52,
                    Carbohydrates = 0.1m,
                    Protein = 0.2m,
                    Fat = 0.3m,
                    Type = "MEAT"
                }, "Food retrieved successfully");
        });

        app.MapPost("/", (CreateFoodRequest request) =>
        {
            return Ok(new FoodResponse()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Portion = request.Portion,
                Calories = request.Calories,
                Carbohydrates = request.Carbohydrates,
                Protein = request.Protein,
                Fat = request.Fat,
                Type = request.Type
            }, "Food created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateFoodRequest>>();
    }
}