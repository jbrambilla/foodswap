using Carter;
using foodswap.DTOs.FoodDTOs;
using foodswap.Filters;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace foodswap.Endpoints;
public class FoodEndpoints : BaseEndpoint
{
    public FoodEndpoints()
        :base("/foods")
    {
        WithTags("Foods");
        RequireAuthorization("AdminOrUser");
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
        .Produces<ApiResponse<List<FoodResponse>>>(200)
        .WithSummaryAndDescription("Retrieve all Foods", "Retrieve all Foods");

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
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Retrieve a Food by Id", "Retrieve a specific Food by it's associated Id")
        .Produces<ApiResponse<FoodResponse>>(200)
        .Produces<ApiResponse<object>>(404, "application/json");

        app.MapPost("/", (CreateFoodRequest request) =>
        {
            return Created(new FoodResponse()
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
        .AddEndpointFilter<ValidatorFilter<CreateFoodRequest>>()
        .WithSummaryAndDescription("Create a new Food", "Create a new Food in the database")
        .Accepts<CreateFoodRequest>("application/json")
        .Produces<ApiResponse<FoodResponse>>(201)
        .Produces<ApiResponse<object>>(400);

    }
}