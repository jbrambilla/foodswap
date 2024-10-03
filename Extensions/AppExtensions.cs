using System.Text.Json;
using foodswap;
using foodswap.Endpoints;
using foodswap.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Extensions;

public static class AppExtensions
{
    public static WebApplication UseArchtectures(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpLogging();

        return app;
    }

    public static WebApplication UseGlobalErrorHandler(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (Exception ex)
            {
                Log.ForContext("HttpMethod", context.Request.Method) 
                    .ForContext("Path", context.Request.Path) 
                    .Error(ex, "Unexpected error in application.");

                context.Response.StatusCode = 500;
                var errorMessage = "An unexpected error has occurred in the server.";

                if (ex.InnerException is JsonException jsonEx)
                {
                    context.Response.StatusCode = 400;  // Retorna BadRequest
                    context.Response.ContentType = "application/json";

                    errorMessage = "Invalid input format. Please check the data types of your request.";
                }             

                await context.Response.WriteAsJsonAsync(new ApiResponse<object>(false, errorMessage, null!, [])); //não expor exception pro usuario
            }
        });

        return app;
    }
    
    public static WebApplication UseCustomUnauthorizedMiddleware(this WebApplication app)
    {
        app.UseStatusCodePages(async context => 
        {
            var response = context.HttpContext.Response;

            if (response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                response.ContentType = "application/json";

                await response.WriteAsJsonAsync(
                    new ApiResponse<object>(
                        false,
                        "User authentication failed.",
                        null!,
                        new List<string> { "Invalid Token" }));
            } else if (response.StatusCode == StatusCodes.Status403Forbidden)
            {
                response.ContentType = "application/json";

                await response.WriteAsJsonAsync(
                    new ApiResponse<object>(
                    false,
                    "Access Denied",
                    null!,
                    new List<string> { "User doesn't have the necessary permissions" }));
            }
        });

        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
    
    public static async Task UseIdentitySeed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();

            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await IdentitySeedData.SeedRoles(roleManager, configuration);
                await IdentitySeedData.SeedAdminUser(userManager, configuration);
            }
            catch (Exception ex)
            {
                // Lidar com exceções relacionadas ao seeding
                Console.WriteLine($"Erro ao realizar o seeding de dados: {ex.Message}");
            }
        }
    }
}