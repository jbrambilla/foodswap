using Carter;
using Extensions;
using Serilog;


try {
    var builder = WebApplication.CreateBuilder(args);

    builder
        .AddLog()
        .AddOptions()
        .AddSwaggerWithAuth()
        .AddServices()
        .AddCarter()
        .AddHttpLogging()
        .AddIdentity();
        
    var app = builder.Build();

    app
        .UseArchtectures()
        .UseGlobalErrorHandler()
        .UseCustomUnauthorizedMiddleware()
        .MapCarter();

    await app.UseIdentitySeed();

    app.Run();
}
catch (Exception ex) {
    Log.Fatal(ex, "Application failed to start");
}
finally {
    Log.CloseAndFlush();
}