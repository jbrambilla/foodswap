using foodswap.Common.Api;
using foodswap.Common.Extensions;
using foodswap.Common.Filters;
using foodswap.Data.Application;
using foodswap.Data.Identity;
using foodswap.Features.SwapperFeatures.DTOs;
using foodswap.tests.Features.SwapperFeatures.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        app.MapGet("/", async (UserManager<User> userManager, HttpContext httpContext, AppDbContext db) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);

            if (user is null) {
                Results.Unauthorized();
            }

            var swappers = await db.Swappers.AsNoTracking().ToListAsync();
            return Ok(swappers.Adapt<List<SwapperResponse>>(), "Swappers retrieved successfully");
        })
        .Produces<ApiResponse<List<SwapperResponse>>>(200)
        .WithSummaryAndDescription("Retrieve all Swappers", "Retrieve all Swappers from the authenticated User")
        .RequireAuthorization("AdminOrUser");

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
        .Produces<ApiResponse<SwapperResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPut("/{id}", (Guid id, CreateOrUpdateSwapperRequest request) =>
        {
            return Results.Ok();
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateSwapperRequest>>()
        .WithSummaryAndDescription("Update Swapper", "Update Swapper in the database")
        .Accepts<CreateOrUpdateSwapperRequest>("application/json")
        .Produces<ApiResponse<SwapperResponse>>(200)
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

        app.MapPatch("/{swapperId}/foodswaps/{foodswapId}/update-servingSize", (Guid swapperId, Guid foodswapId, UpdateFoodSwapServingSizeRequest request) =>
        {
            return Results.Ok(request);
        })
        .AddEndpointFilter<ValidatorFilter<UpdateFoodSwapServingSizeRequest>>()
        .Accepts<UpdateFoodSwapServingSizeRequest>("application/json")
        .Produces<ApiResponse<FoodSwapResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapDelete("/{swapperId}/foodswaps/{foodswapId}", (Guid swapperId, Guid foodswapId) =>
        {
            return Results.Ok();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);
    }
}