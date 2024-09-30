public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseAppMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}