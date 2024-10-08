using Carter;
using foodswap.Common.Api;
using foodswap.Common.Filters;
using foodswap.Common.Services;
using foodswap.Data.Identity;
using foodswap.Features.UserFeatures.UserDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Serilog;

namespace foodswap.Endpoints;
public class UserEndpoints : BaseEndpoint
{
    public UserEndpoints()
        : base("api/v1/users")
    {
        WithTags("Users");
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateUserRequest request, UserManager<User> userManager, EmailService emailService) =>
        {
            var user = new User(request.Name, request.Email);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Error creating user");
            }

            var resultAddRole = await userManager.AddToRoleAsync(user, "user");
            if (!resultAddRole.Succeeded) {
                Log.Error("Error adding role to the user {user}: {Error}", user.Email, resultAddRole.Errors);
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailService.SendEmailAsync(user.Email!, "Confirme seu e-mail", $"Token: {token}");

            return Ok(user, "User created successfully");
        })
        .AddEndpointFilter<ValidatorFilter<CreateUserRequest>>();

        app.MapPost("/forgot-password", async (ForgotPasswordRequest request, UserManager<User> userManager, EmailService emailService) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                Log.ForContext("Email", request.Email)
                    .ForContext("Path", "forgot-password")
                    .Error("User tried to reset password but not found with email {Email}", request.Email);
                return Ok("If this e-mail is registered in our platform, you will receive an e-mail with a link to reset your password");
            }

            if (!user.EmailConfirmed) {
                return BadRequest(["E-mail not confirmed, try asking for a new e-mail confirmation"], "Error resetting password");
            }

            var resetCode = await userManager.GeneratePasswordResetTokenAsync(user);

            await emailService.SendEmailAsync(user.Email!, "Redefinição de senha", $"Reset code: {resetCode}");

            return Ok("If this e-mail is registered in our platform, you will receive an e-mail with a link to reset your password");
        })
        .AddEndpointFilter<ValidatorFilter<ForgotPasswordRequest>>();

        app.MapPost("/reset-password", async (UserResetPasswordRequest request, UserManager<User> userManager) =>
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) {
                return BadRequest(["User not found"], "Error resetting password");
            }

            var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Error resetting password");
            }

            return Ok("Password changed successfully");
        })
        .AddEndpointFilter<ValidatorFilter<ResetPasswordRequest>>();

        app.MapPost("/change-password", async (ChangePasswordRequest request, UserManager<User> userManager, HttpContext httpContext) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null) {
                return Results.Unauthorized();
            }

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors, "Error changing password");

            return Ok("Password changed successfully");
        })
        .AddEndpointFilter<ValidatorFilter<ChangePasswordRequest>>()
        .RequireAuthorization("AdminOrUser");

        app.MapGet("/request-email-confirmation", async (UserManager<User> userManager, HttpContext httpContext, EmailService emailService) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null) {
                return Results.Unauthorized();
            }

            if (user.EmailConfirmed) {
                return BadRequest(["This e-mail is already confirmed"], "Email já confirmado");
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailService.SendEmailAsync(user.Email!, "Confirme seu e-mail", $"Token: {token} UserId: {user.Id}");

            return Ok("An e-mail was sent with a confirmation link to confirm your e-mail");
        })
        .RequireAuthorization("AdminOrUser");

        //TODO: criar validator
        app.MapPost("/request-email-change", async (RequestEmailChangeRequest request, UserManager<User> userManager, HttpContext httpContext, EmailService emailService) =>
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            if (user == null) {
                return Results.Unauthorized();
            }

            var token = await userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

            await emailService.SendEmailAsync(request.NewEmail, "Confirme seu novo e-mail", $"Token: {token} UserId: {user.Id}");
            return Ok("An e-mail was sent with a confirmation link to confirm your e-mail");

            //return Ok(new {userId = user.Id, request.NewEmail, token}, "An e-mail was sent with a confirmation link to confirm your e-mail");
        })
        .RequireAuthorization("AdminOrUser");

        // TODO: criar validator
        app.MapPost("/change-email", async (ChangeEmailRequest request, UserManager<User> userManager, HttpContext httpContext, AuthDbContext authDbContext) =>
        {
            using (var transaction = await authDbContext.Database.BeginTransactionAsync()) 
            {
                var user = await userManager.FindByIdAsync(request.UserId);
                if (user == null) {
                    return BadRequest(["User not found"], "Error changing e-mail");
                }

                var changeEmailResult = await userManager.ChangeEmailAsync(user, request.NewEmail, request.Token);
                if (!changeEmailResult.Succeeded) {
                    return BadRequest(changeEmailResult.Errors, "Error changing e-mail");
                }

                var setUserNameResult = await userManager.SetUserNameAsync(user, request.NewEmail);
                if (!setUserNameResult.Succeeded) {
                    return BadRequest(setUserNameResult.Errors, "Error changing e-mail");
                }

                await transaction.CommitAsync();
                return Ok("E-mail changed successfully");
            }
        });

        app.MapPost("/confirm-email", async (ConfirmEmailRequest request, UserManager<User> userManager) =>
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null) {
                return BadRequest(["User not found"], "Error confirming e-mail");
            }

            var result = await userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Error confirming e-mail");
            }

            return Ok("E-mail confirmed successfully");
        })
        .AddEndpointFilter<ValidatorFilter<ConfirmEmailRequest>>();
    }
}