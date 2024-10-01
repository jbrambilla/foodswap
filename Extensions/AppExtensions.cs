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
                await context.Response.WriteAsJsonAsync("An unexpected error has occurred in the server."); //não expor ex pro usuario
            }
        });

        return app;
    }

    public static WebApplication UseLogRequestHandler(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            Log.ForContext("HttpMethod", context.Request.Method) 
            .ForContext("Path", context.Request.Path) 
            .Information($"REQUEST {context.Request.Method} {context.Request.Path} START");

            await next();

            Log.ForContext("HttpMethod", context.Request.Method)
            .ForContext("Path", context.Request.Path)
            .ForContext("StatusCode", context.Response.StatusCode)
            .Information($"REQUEST {context.Request.Method} {context.Request.Path} FINISHED WITH Status: {context.Response.StatusCode}");
        });

        return app;
    }
}