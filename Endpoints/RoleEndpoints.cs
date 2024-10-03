using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace foodswap.Endpoints;

public class RoleEndpoints : CarterModule
{
    public RoleEndpoints()
        : base("/roles")
    {
        WithTags("Roles");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateRoleRequest request, RoleManager<IdentityRole> roleManager) =>
        {
            if (string.IsNullOrEmpty(request.Name)) {
                return Results.BadRequest("The Name of the Role is required");
            }

            var result = await roleManager.CreateAsync(new IdentityRole(request.Name));

            if (!result.Succeeded) {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok("Role created successfully");
        });

        app.MapGet("/", async (RoleManager<IdentityRole> roleManager) =>
        {
            var roles = await roleManager.Roles.ToListAsync();
            return Results.Ok(roles);
        });
    }
}