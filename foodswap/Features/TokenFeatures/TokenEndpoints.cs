using Carter;
using foodswap.Common.Api;
using foodswap.Common.Filters;
using foodswap.Data.Identity;
using foodswap.Features.TokenFeatures.TokenDTOs;
using Microsoft.AspNetCore.Identity;

namespace foodswap.Features.TokenFeatures;

public class TokenEndpoints : BaseEndpoint
{
    public TokenEndpoints()
        : base("api/v1/token")
    {
        WithTags("Token");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (GetTokenRequest request, UserManager<User> userManager, TokenProvider tokenProvider) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null) {
                return BadRequest(["Invalid email or password"], "User authentication failed");
            }

            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result) {
                return BadRequest(["Invalid email or password"], "User authentication failed");
            }

            var roles = await userManager.GetRolesAsync(user);

            var token = tokenProvider.Create(user, roles.ToArray());
            
            return Ok(new TokenResponse{ Token = token, ExpiresAt = DateTime.Now.AddMinutes(60) }, "Token created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<GetTokenRequest>>();
    }
}

