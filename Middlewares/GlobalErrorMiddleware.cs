using Serilog;

public static class GlobalErrorMiddleware
{
    public static void UseGlobalErrorHandler(this IApplicationBuilder app)
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
    }
}