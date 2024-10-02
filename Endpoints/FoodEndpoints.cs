using Carter;
using foodswap.DTOs.FoodDTOs;
using foodswap.Filters;

namespace foodswap.Endpoints;
public class FoodEndpoints : CarterModule
{
    public FoodEndpoints()
        :base("/foods")
    {
        WithTags("Foods");
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            return Results.Ok(new FoodResponse(){
                Id = Guid.NewGuid(),
                Name = "apple",
                Calories = 52,
                Carbohydrates = 0.1m,
                Protein = 0.2m,
                Fat = 0.3m});
        });

        app.MapGet("/{id}", (Guid id) =>
        {
            return Results.Ok(new FoodResponse(){
                Id = id,
                Name = "apple",
                Calories = 52,
                Carbohydrates = 0.1m,
                Protein = 0.2m,
                Fat = 0.3m});
        });

        app.MapPost("/", (CreateFoodRequest request) =>
        {
            return Results.Ok(new FoodResponse(){
                Id = Guid.NewGuid(),
                Name = request.Name,
                Calories = request.Calories,
                Carbohydrates = request.Carbohydrates,
                Protein = request.Protein,
                Fat = request.Fat
            });
        })
        .AddEndpointFilter<ValidatorFilter<CreateFoodRequest>>();
    }
}