using Carter;
using foodswap.Common.Extensions;
using Serilog;


try {
    var builder = WebApplication.CreateBuilder(args);

    builder
        .AddLog()
        .AddOptions()
        .AddArchtectures()
        .AddSwaggerWithAuth()
        .AddServices()
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