using Serilog;

public static class EnrichRequestLogMiddleware
{
    public static IApplicationBuilder UseEnrichLogRequest(this IApplicationBuilder app)
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