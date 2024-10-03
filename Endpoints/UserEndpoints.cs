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
        app.MapPost("/", async (CreateUserRequest request, UserManager<User> userManager) =>
        {
            var user = new User(request.Name, request.Surname, request.Email, request.PhoneNumber, request.BirthDate);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) {
                return Results.BadRequest(result.Errors);
            }
            return Results.Ok("User created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateUserRequest>>();

        app.MapPost("/token", async (GetTokenRequest request, UserManager<User> userManager, TokenProvider tokenProvider) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return Results.BadRequest("Invalid email or password");
            }

            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result) {
                return Results.BadRequest("Invalid email or password");
            }

            var token = tokenProvider.Create(user);
            return Results.Ok(token);
        })
        .AddEndpointFilter<ValidatorFilter<GetTokenRequest>>();
    }
}