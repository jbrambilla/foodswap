using Carter;
using foodswap.DTOs.UserDTOs;
using foodswap.Filters;
using foodswap.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace foodswap.Endpoints;
public class UserEndpoints : BaseEndpoint
{
    public UserEndpoints()
        : base("/users")
    {
        WithTags("Users");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateUserRequest request, UserManager<User> userManager) =>
        {
            var user = new User(request.Name, request.Surname, request.Email, request.PhoneNumber, request.BirthDate);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Error creating user");
            }

            var resultAddRole = await userManager.AddToRoleAsync(user, "user");
            if (!resultAddRole.Succeeded) {
                Log.Error("Error adding role to the user {user}: {Error}", user.Email, resultAddRole.Errors);
            }
            return Ok(user, "User created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateUserRequest>>();
    }
}