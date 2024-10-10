using System.Security.Claims;
using foodswap.Common.Api;
using foodswap.Common.Extensions;
using foodswap.Common.Filters;
using foodswap.Data.Application;
using foodswap.Data.Identity;
using foodswap.Features.SwapperFeatures.DTOs;
using foodswap.Features.SwapperFeatures.Models;
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
        //RequireAuthorization("AdminOrUser");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (UserManager<User> userManager, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) {
                Results.Unauthorized();
            }

            var swappers = await db.Swappers
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .ToListAsync();
            return Ok(swappers.Adapt<List<SwapperResponse>>(), "Swappers retrieved successfully");
        })
        .Produces<ApiResponse<List<SwapperResponse>>>(200)
        .WithSummaryAndDescription("Retrieve all Swappers", "Retrieve all Swappers from the authenticated User");

        app.MapGet("/{id}", async (Guid id, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) {
                Results.Unauthorized();
            }

            var swappers = await db.Swappers
                .AsNoTracking()
                .Include(s => s.FoodSwaps)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Id == id);

            if (swappers is null) {
                return BadRequest(["The Swapper was not found in the database"], "Swapper not found");
            }

            return Ok(swappers.Adapt<GetSwapperByIdResponse>(), "Swapper retrieved successfully");
        })
        .WithSummaryAndDescription("Get Swapper by Id", "Get a swapper by the Id from the database and bring all foodswaps with it")
        .WithIdDescription("The Id associated with the created Swapper")
        .Produces<ApiResponse<GetSwapperByIdResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPost("/", async (CreateOrUpdateSwapperRequest request, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            var swapper = new Swapper(userId, request.Name, request.Description);
            db.Swappers.Add(swapper);
            await db.SaveChangesAsync();

            return Created(swapper.Adapt<SwapperResponse>(), "Swapper created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateSwapperRequest>>()
        .WithSummaryAndDescription("Create a new Swapper", "Create a new Swapper in the database")
        .Accepts<CreateOrUpdateSwapperRequest>("application/json")
        .Produces<ApiResponse<SwapperResponse>>(201)
        .Produces<ApiResponse<object>>(400);

        app.MapPut("/{id}", async (Guid id, CreateOrUpdateSwapperRequest request, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            var swapper = await db.Swappers.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (swapper is null) {
                return BadRequest(["The Swapper was not found in the database or you are not the owner"], "Swapper not found");
            }

            swapper.Update(request.Name, request.Description);
            await db.SaveChangesAsync();

            return Ok(swapper.Adapt<SwapperResponse>(), "Swapper updated successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateSwapperRequest>>()
        .WithSummaryAndDescription("Update Swapper", "Update Swapper in the database")
        .Accepts<CreateOrUpdateSwapperRequest>("application/json")
        .Produces<ApiResponse<SwapperResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapDelete("/{id}", async (Guid id, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            var swapper = await db.Swappers.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (swapper is null) {
                return BadRequest(["The Swapper was not found in the database or you are not the owner"], "Swapper not found");
            }

            db.Swappers.Remove(swapper);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);

        app.MapPost("/{swapperId}/foodswaps", async (Guid swapperId, CreateFoodSwapRequest request, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            if (!db.Swappers.AsNoTracking().Any(s => s.Id == swapperId && s.UserId == userId)) {
                return BadRequest(["Swapper does not exist or does not belong to the authenticated User"], "Swapper does not exist in the database");
            }

            var foodSwap = new FoodSwap(swapperId, request.Name, request.Category, request.ServingSize, request.Calories, request.Carbohydrates, request.Protein, request.Fat);
            db.FoodSwaps.Add(foodSwap);
            await db.SaveChangesAsync();

            return Ok(foodSwap.Adapt<FoodSwapResponse>(), "FoodSwap created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateFoodSwapRequest>>()
        .WithSummaryAndDescription("Create a new FoodSwap", "Create a new FoodSwap in the database")
        .Accepts<CreateFoodSwapRequest>("application/json")
        .Produces<ApiResponse<FoodSwapResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapPatch("/{swapperId}/foodswaps/{foodswapId}/define-main", async (Guid swapperId, Guid foodswapId, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            if (!db.Swappers.AsNoTracking().Any(s => s.Id == swapperId && s.UserId == userId)) {
                return BadRequest(["Swapper does not exist or does not belong to the authenticated User"], "Swapper does not exist in the database");
            }

            var foodSwap = db.FoodSwaps.FirstOrDefault(fs => fs.SwapperId == swapperId && fs.Id == foodswapId);
            if (foodSwap is null) {
                return BadRequest(["FoodSwap does not exist or does not belong to the authenticated Swapper"], "FoodSwap does not exist in the database");
            }

            using var transaction = await db.Database.BeginTransactionAsync();
            try 
            {
                await db.FoodSwaps
                    .Where(fs => fs.SwapperId == swapperId && fs.IsMain)
                    .ExecuteUpdateAsync(fs => fs.SetProperty(fs => fs.IsMain, false));

                foodSwap.DefineAsMain();

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return Results.NoContent();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }            
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);

        app.MapPatch("/{swapperId}/foodswaps/{foodswapId}/update-servingSize", async (Guid swapperId, Guid foodswapId, UpdateFoodSwapServingSizeRequest request, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            if (!db.Swappers.AsNoTracking().Any(s => s.Id == swapperId && s.UserId == userId)) {
                return BadRequest(["Swapper does not exist or does not belong to the authenticated User"], "Swapper does not exist in the database");
            }

            var foodSwap = db.FoodSwaps.FirstOrDefault(fs => fs.SwapperId == swapperId && fs.Id == foodswapId);
            if (foodSwap is null) {
                return BadRequest(["FoodSwap does not exist or does not belong to the authenticated Swapper"], "FoodSwap does not exist in the database");
            }

            foodSwap.UpdateServingSize(request.ServingSize);
            await db.SaveChangesAsync();
            return Ok(foodSwap.Adapt<FoodSwapResponse>(), "FoodSwap updated successfully");
        })
        .AddEndpointFilter<ValidatorFilter<UpdateFoodSwapServingSizeRequest>>()
        .Accepts<UpdateFoodSwapServingSizeRequest>("application/json")
        .Produces<ApiResponse<FoodSwapResponse>>(200)
        .Produces<ApiResponse<object>>(400);

        app.MapDelete("/{swapperId}/foodswaps/{foodswapId}", async (Guid swapperId, Guid foodswapId, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) {
                return Results.Unauthorized();
            }

            if (!db.Swappers.AsNoTracking().Any(s => s.Id == swapperId && s.UserId == userId)) {
                return BadRequest(["Swapper does not exist or does not belong to the authenticated User"], "Swapper does not exist in the database");
            }

            var foodSwap = db.FoodSwaps.FirstOrDefault(fs => fs.SwapperId == swapperId && fs.Id == foodswapId);
            if (foodSwap is null) {
                return BadRequest(["FoodSwap does not exist or does not belong to the authenticated Swapper"], "FoodSwap does not exist in the database");
            }

            db.Remove(foodSwap);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .Produces(204)
        .Produces<ApiResponse<object>>(400);

        app.MapGet("/{swapperId}/foodswaps/{foodswapId}/suggestions", async (Guid swapperId, Guid foodswapId, ClaimsPrincipal user, AppDbContext db, [AsParameters]GetSuggestionsRequest request) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) {
                return Results.Unauthorized();
            }

            if (!db.Swappers.AsNoTracking().Any(s => s.Id == swapperId && s.UserId == userId)) {
                return BadRequest(["Swapper does not exist or does not belong to the authenticated User"], "Swapper does not exist in the database");
            }

            var foodSwap = db.FoodSwaps.FirstOrDefault(fs => fs.SwapperId == swapperId && fs.Id == foodswapId);
            if (foodSwap is null) {
                return BadRequest(["FoodSwap does not exist or does not belong to the authenticated Swapper"], "FoodSwap does not exist in the database");
            }

            if (!foodSwap.IsMain) return BadRequest(["FoodSwap is not main"], "FoodSwap is not main");

            var take = (int)(request.PageSize is null ? 10 : request.PageSize);
            var skip = (int)(request.Page is null ? 0 : (request.Page - 1) * take);

            var errorMargin = 20M;

            var minPortion = foodSwap.ServingSize - (foodSwap.ServingSize * 0.5M);
            var maxPortion = foodSwap.ServingSize + (foodSwap.ServingSize * 0.5M);

            var query = db.Foods
                .AsNoTracking()
                .Where(f => f.Name != foodSwap.Name)
                .Where(f =>
                    Enumerable.Range((int)minPortion, (int)(maxPortion - minPortion + 1))
                    .Any(comparedPortion =>
                        Math.Abs(foodSwap.Calories - f.CaloriesPerGram * comparedPortion) <= errorMargin
                        && Math.Abs(foodSwap.Carbohydrates - f.CarbohydratesPerGram * comparedPortion) <= errorMargin
                        && Math.Abs(foodSwap.Protein - f.ProteinPerGram * comparedPortion) <= errorMargin
                        && Math.Abs(foodSwap.Fat - f.FatPerGram * comparedPortion) <= errorMargin
                        //&& RelatedCategories.GetRelatedCategories(foodSwap.Category).Contains(f.Category)
                    )
            );

            var foods = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var count = await query.CountAsync();

            return Ok(new GetSuggestionResponse{ 
                Page = request.Page ?? 1,
                PageSize = request.PageSize ?? 10,
                Count = count,
                Foods = foods.Adapt<List<FoodSuggestionResponse>>()
            });
        })
        .Produces<ApiResponse<List<GetSuggestionResponse>>>(200)
        .Produces<ApiResponse<object>>(400)
        .WithSummaryAndDescription("Retrieve Foods as suggestions for the selected food", "The suggestion is made based on macros and related categories");
    }
}

public static class RelatedCategories
{
    public static List<EFoodCategory> GetRelatedCategories(EFoodCategory category)
    {
        var relatedCategories = new Dictionary<int, List<EFoodCategory>>()
        {
            { 1, new List<EFoodCategory> { EFoodCategory.GRAIN, EFoodCategory.VEGETABLES, EFoodCategory.SUGARY } },
            { 2, new List<EFoodCategory> { EFoodCategory.MEATS, EFoodCategory.SEAFOODS, EFoodCategory.LEGUMINOUS } },
            { 3, new List<EFoodCategory> { EFoodCategory.DAIRY, EFoodCategory.DAIRY, EFoodCategory.SEEDS } }
        };

        return relatedCategories
            .Where(r => r.Value.Contains(category))
            .SelectMany(r => r.Value)
            .Distinct()
            .ToList();
    }
}