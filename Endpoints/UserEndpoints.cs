using System.Security.Claims;
using Carter;
using foodswap.Business.Interfaces.Services;
using foodswap.DTOs.UserDTOs;
using foodswap.Filters;
using foodswap.Identity;
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
        app.MapPost("/", async (CreateUserRequest request, UserManager<User> userManager) =>
        {
            var user = new User(request.Name, request.Email);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Erro ao criar usuario");
            }

            var resultAddRole = await userManager.AddToRoleAsync(user, "user");
            if (!resultAddRole.Succeeded) {
                Log.Error("Error adding role to the user {user}: {Error}", user.Email, resultAddRole.Errors);
            }
            return Ok(user, "Usuario criado com sucesso");
        })
        .AddEndpointFilter<ValidatorFilter<CreateUserRequest>>();

        app.MapPost("/forgot-password", async (ForgotPasswordRequest request, UserManager<User> userManager, IEmailService emailService) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                Log.ForContext("Email", request.Email)
                    .ForContext("Path", "forgot-password")
                    .Error("User tried to reset password but not found with email {Email}", request.Email);
                return Ok("Se este e-mail estiver cadastrado em nossa plataforma, você receberá um link para redefinir sua senha");
            }

            if (!user.EmailConfirmed) {
                return BadRequest(["Email não confirmado. Tente pedir um novo e-mail de confirmação."], "Erro ao redefinir senha");
            }

            var resetCode = await userManager.GeneratePasswordResetTokenAsync(user);

            await emailService.SendEmailAsync(user.Email!, "Redefinição de senha", $"Reset code: {resetCode}");

            return Ok("Se este e-mail estiver cadastrado em nossa plataforma, você receberá um link para redefinir sua senha");
        })
        .AddEndpointFilter<ValidatorFilter<ForgotPasswordRequest>>();

        app.MapPost("/reset-password", async (ResetPasswordRequest request, UserManager<User> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                return BadRequest(["Usuário não encontrado"], "Erro ao redefinir senha");
            }

            var result = await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Erro ao redefinir senha");
            }

            return Ok("Senha redefinida com sucesso");
        })
        .AddEndpointFilter<ValidatorFilter<ResetPasswordRequest>>();

        app.MapPost("/change-password", async (ChangePasswordRequest request, UserManager<User> userManager, HttpContext httpContext) =>
        {
            var user = await GetUser(userManager, httpContext);
            if (user == null)
            {
                return Results.Unauthorized();
            }

            var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded) return BadRequest(result.Errors, "Erro ao alterar senha");

            return Ok("Senha alterada com sucesso");
        })
        .AddEndpointFilter<ValidatorFilter<ChangePasswordRequest>>()
        .RequireAuthorization("AdminOrUser");

        app.MapGet("/send-emailconfirmationtoken", async (UserManager<User> userManager, HttpContext httpContext, IEmailService emailService) =>
        {
            var user = await GetUser(userManager, httpContext);
            if (user == null) {
                return Results.Unauthorized();
            }

            if (user.EmailConfirmed) {
                return BadRequest(["Este e-mail já foi confirmado pelo usuário"], "Email já confirmado");
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await emailService.SendEmailAsync(user.Email!, "Confirme seu e-mail", $"Token: {token}");

            return Ok("Um e-mail com o link de confirmação foi enviado para o seu e-mail");
        })
        .RequireAuthorization("AdminOrUser");

        app.MapPost("/confirm-email", async (ConfirmEmailRequest request, UserManager<User> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null) {
                return BadRequest(["Usuário não encontrado"], "Erro ao confirmar e-mail");
            }

            var result = await userManager.ConfirmEmailAsync(user, request.ConfirmationToken);
            if (!result.Succeeded) {
                return BadRequest(result.Errors, "Erro ao confirmar e-mail");
            }

            return Ok("E-mail confirmado com sucesso!");
        })
        .AddEndpointFilter<ValidatorFilter<ConfirmEmailRequest>>();
    }

    private async Task<User> GetUser(UserManager<User> userManager, HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return null!;
        }

        var user = await userManager.FindByIdAsync(userId);
        return user!;
    }
}