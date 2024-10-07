using Carter;
using foodswap.Common.Api;
using foodswap.Common.Extensions;
using foodswap.Common.Filters;
using foodswap.Features.FoodFeatures.FoodDTOs;
using Mapster;

namespace foodswap.Features.FoodFeatures;
public class FoodEndpoints : BaseEndpoint
{
    public FoodEndpoints()
        :base("api/v1/foods")
    {
        WithTags("Foods");
        //RequireAuthorization("AdminOrUser");
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            return Ok(
                new List<FoodResponse>()
                {
                    new FoodResponse()
                    {
                        Id = Guid.NewGuid(),
                        Name = "apple",
                        ServingSize = 100,
                        Calories = 52,
                        Carbohydrates = 11m,
                        Protein = 5m,
                        Fat = 3m,
                        Category = "VEGETABLE"
                    }
                }, "Foods retrieved successfully");
        })
        .Produces<ApiResponse<List<FoodResponse>>>(200)
        .WithSummaryAndDescription("Retrieve all Foods", "Retrieve all Foods");

        app.MapGet("/{id}", (Guid id) =>
        {

            return Ok(
                new FoodResponse() 
                {
                    Id = id,
                    Name = "apple",
                    ServingSize = 100,
                    Calories = 52,
                    Carbohydrates = 0.1m,
                    Protein = 0.2m,
                    Fat = 0.3m,
                    Category = "MEAT"
                }, "Food retrieved successfully");
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Retrieve a Food by Id", "Retrieve a specific Food by it's associated Id")
        .Produces<ApiResponse<FoodResponse>>(200)
        .Produces<ApiResponse<object>>(404, "application/json");

        app.MapPost("/", (CreateFoodRequest request) =>
        {
            var food = new Food(request.Name, request.ServingSize, request.Calories, request.Carbohydrates, request.Protein, request.Fat, request.Category);
            return Created(food.Adapt<FoodResponse>(), "Food created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateFoodRequest>>()
        .WithSummaryAndDescription("Create a new Food", "Create a new Food in the database")
        .Accepts<CreateFoodRequest>("application/json")
        .Produces<ApiResponse<FoodResponse>>(201)
        .Produces<ApiResponse<object>>(400);

    }
}