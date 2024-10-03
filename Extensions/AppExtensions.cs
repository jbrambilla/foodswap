using System.Text.Json;
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

        app.UseAuthentication();
        app.UseAuthorization();

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

                await context.Response.WriteAsJsonAsync(errorMessage); //não expor exception pro usuario
            }
        });

        return app;
    }
}