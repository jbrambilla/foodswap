using Carter;
using foodswap.Common.Api;
using foodswap.Features.RoleFeatures.RoleDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace foodswap.Endpoints;

public class RoleEndpoints : BaseEndpoint
{
    public RoleEndpoints()
        : base("api/v1/roles")
    {
        WithTags("Roles");
        RequireAuthorization("Admin");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateRoleRequest request, RoleManager<IdentityRole> roleManager) =>
        {
            if (string.IsNullOrEmpty(request.Name)) {
                return BadRequest(["The Name of the Role is required"], "Error creating role");
            }

            var result = await roleManager.CreateAsync(new IdentityRole(request.Name));

            if (!result.Succeeded) {
                //return Results.BadRequest(result.Errors);
                return BadRequest(result.Errors, "Error creating role");
            }

            return Ok(request, "Role created successfully");
        })
        .ExcludeFromDescription();

        app.MapGet("/", async (RoleManager<IdentityRole> roleManager) =>
        {
            var roles = await roleManager.Roles.ToListAsync();
            return Ok(roles, "Roles retrieved successfully");
        })
        .ExcludeFromDescription();
    }
}