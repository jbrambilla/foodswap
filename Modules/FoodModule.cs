using Carter;
using foodswap.DTOs;
using foodswap.Filters;

namespace foodswap.Modules;
public class FoodModule : CarterModule
{
    public FoodModule()
        :base("/food")
    {
        WithTags("Food");
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

        app.MapPost("/", (FoodRequest request) =>
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
        .AddEndpointFilter<ValidatorFilter<FoodRequest>>();
    }
}