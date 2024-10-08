using Carter;
using foodswap.Common.Api;
using foodswap.Common.Extensions;
using foodswap.Common.Filters;
using foodswap.Data.Application;
using foodswap.Features.FoodFeatures.FoodDTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace foodswap.Features.FoodFeatures;
public class FoodEndpoints : BaseEndpoint
{
    public FoodEndpoints()
        :base("api/v1/foods")
    {
        WithTags("Foods");
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async ([AsParameters]GetAllFoodRequest request, AppDbContext db) =>
        {
            var query = db.Foods
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Name))
                query = query.Where(f => f.Name.Contains(request.Name));

            if (request.Sort is not null && request.Sort.ToLower() == "desc")
                query = query.OrderByDescending(f => f.Name);
            else query = query.OrderBy(f => f.Name);

            var take = (int)(request.PageSize is null ? 10 : request.PageSize);
            var skip = (int)(request.Page is null ? 0 : (request.Page - 1) * take);

            var foods = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var count = await query.CountAsync();

            var response = new GetAllFoodResponse
            {
                Page = request.Page ?? 1,
                PageSize = request.PageSize ?? 10,
                Count = count,
                Foods = foods.Adapt<List<FoodResumedResponse>>()

            };
            return Ok(
                response, "Foods retrieved successfully");
        })
        .Produces<ApiResponse<List<GetAllFoodResponse>>>(200)
        .WithSummaryAndDescription("Retrieve all Foods", "Retrieve all Foods")
        .RequireAuthorization("AdminOrUser");

        app.MapGet("/{id}", (Guid id, AppDbContext db) =>
        {
            var food = db.Foods.AsNoTracking().FirstOrDefault(f => f.Id == id);

            if (food is null){
                return BadRequest(["No food found with the specified Id"], "Food not found");
            }

            return Ok(food.Adapt<FoodResumedResponse>(), "Food retrieved successfully");
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Retrieve a Food by Id", "Retrieve a specific Food by it's associated Id")
        .Produces<ApiResponse<FoodResumedResponse>>(200)
        .Produces<ApiResponse<object>>(404, "application/json")
        .RequireAuthorization("AdminOrUser");

        app.MapPost("/", async (CreateOrUpdateFoodRequest request, AppDbContext db) =>
        {
            if (db.Foods.AsNoTracking().Any(f => f.Name == request.Name)) {
                return BadRequest(["This food already exists in the database"], "Food already exists");
            }

            var food = new Food(request.Name, request.ServingSize, request.Calories, request.Carbohydrates, request.Protein, request.Fat, request.Category);

            db.Foods.Add(food);
            await db.SaveChangesAsync();

            return Created(food.Adapt<FoodResumedResponse>(), "Food created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateFoodRequest>>()
        .WithSummaryAndDescription("Create a new Food", "Create a new Food in the database")
        .Accepts<CreateOrUpdateFoodRequest>("application/json")
        .Produces<ApiResponse<FoodResumedResponse>>(201)
        .Produces<ApiResponse<object>>(400)
        .RequireAuthorization("AdminOrUser");

        app.MapPut("/{id}", async (Guid id, CreateOrUpdateFoodRequest request, AppDbContext db) =>
        {
            var food = await db.Foods.FindAsync(id);
            if (food is null) {
                return BadRequest(["The food with the specified Id does not exist"], "Food not found");
            }

            food.Update(request.Name, request.ServingSize, request.Category, request.Calories, request.Carbohydrates, request.Protein, request.Fat);
            await db.SaveChangesAsync();

            return Ok(food.Adapt<FoodResumedResponse>(), "Food updated successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateOrUpdateFoodRequest>>()
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Update a Food", "Update Food data in the database")
        .Accepts<CreateOrUpdateFoodRequest>("application/json")
        .Produces<ApiResponse<FoodResumedResponse>>(200)
        .Produces<ApiResponse<object>>(400)
        .RequireAuthorization("AdminOrUser");

        app.MapPatch("/{id}/activate", async (Guid id, AppDbContext db) =>
        {
            var food = await db.Foods.FindAsync(id);
            if (food is null){
                return BadRequest(["No food found with the specified Id"], "Food not found");
            }

            if (food.IsActive) return Results.NoContent();
            
            food.Activate();
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Activate a Food", "Active a Food that was deactivated in the system")
        .Produces(204)
        .Produces<ApiResponse<object>>(400)
        .RequireAuthorization("Admin");

        app.MapPatch("/{id}/deactivate", async (Guid id, AppDbContext db) =>
        {
            var food = await db.Foods.FindAsync(id);
            if (food is null){
                return BadRequest(["No food found with the specified Id"], "Food not found");
            }

            if (!food.IsActive) return Results.NoContent();

            food.Deactivate();
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Deactivate a Food", "Deactive a Food that is active in the system")
        .Produces(204)
        .Produces<ApiResponse<object>>(400)
        .RequireAuthorization("Admin");
 
        app.MapDelete("/{id}", async (Guid id, AppDbContext db) =>
        {
            var food = await db.Foods.FindAsync(id);
            if (food is null){
                return BadRequest(["No food found with the specified Id"], "Food not found");
            }

            db.Foods.Remove(food);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithIdDescription("The Id associated with the created Food")
        .WithSummaryAndDescription("Remove a Food", "Remove a Food from the database")
        .Produces(204)
        .Produces<ApiResponse<object>>(400)
        .RequireAuthorization("Admin");
    }
}