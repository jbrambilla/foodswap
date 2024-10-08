using foodswap.Common.Api;
using foodswap.Common.Extensions;
using foodswap.Common.Filters;
using foodswap.Features.SwapperFeatures.DTOs;

namespace foodswap.Features.SwapperFeatures;

public class SwapperEndpoints : BaseEndpoint
{
    public SwapperEndpoints()
        :base("api/v1/swappers")
    {
        WithTags("Swappers");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () =>
        {
            return Results.Ok();
        });

        app.MapGet("/{id}", (Guid id) =>
        {
            return Results.Ok();
        })
        .WithSummaryAndDescription("Get Swapper by Id", "Get a swapper by the Id from the database and bring all foodswaps with it")
        .WithIdDescription("The Id associated with the created Swapper")
        .Produces<ApiResponse<GetSwapperByIdResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPost("/", (CreateOrUpdateSwapperRequest request) =>
        {
            return Results.Ok();
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateSwapperRequest>>()
        .WithSummaryAndDescription("Create a new Swapper", "Create a new Swapper in the database")
        .Accepts<CreateOrUpdateSwapperRequest>("application/json")
        .Produces<ApiResponse<CreateOrUpdateSwapperResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPut("/{id}", (Guid id, CreateOrUpdateSwapperRequest request) =>
        {
            return Results.Ok();
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateSwapperRequest>>()
        .WithSummaryAndDescription("Update Swapper", "Update Swapper in the database")
        .Accepts<CreateOrUpdateSwapperRequest>("application/json")
        .Produces<ApiResponse<CreateOrUpdateSwapperResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapDelete("/{id}", (Guid id) =>
        {
            return Results.Ok();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);

        app.MapPost("/{swapperId}/foodswaps", (Guid swapperId, CreateFoodSwapRequest request) =>
        {
            return Results.Ok();
        })
        .AddEndpointFilter<ValidatorFilter<CreateFoodSwapRequest>>()
        .WithSummaryAndDescription("Create a new FoodSwap", "Create a new FoodSwap in the database")
        .Accepts<CreateFoodSwapRequest>("application/json")
        .Produces<ApiResponse<FoodSwapResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPatch("/{swapperId}/foodswaps/{foodswapId}/define-main", (Guid swapperId, Guid foodswapId) =>
        {
            return Results.Ok();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);

        app.MapDelete("/{swapperId}/foodswaps/{foodswapId}", (Guid swapperId, Guid foodswapId) =>
        {
            return Results.Ok();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);
    }
}