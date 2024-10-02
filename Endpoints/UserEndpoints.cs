using Carter;
using foodswap.DTOs.UserDTOs;
using foodswap.Filters;
using foodswap.Identity;
using Microsoft.AspNetCore.Identity;

public class UserEndpoints : CarterModule
{
    public UserEndpoints()
        : base("/users")
    {
        WithTags("Users");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", (CreateUserRequest request, UserManager<User> userManager) =>
        {
            var user = new User(request.Name, request.Surname, request.Email, request.PhoneNumber, request.BirthDate);
            var result = userManager.CreateAsync(user, request.Password).Result;
            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok("User created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateUserRequest>>();
    }
}