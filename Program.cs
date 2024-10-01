using Carter;
using Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchtectures()
    .AddServices()
    .AddHttpLogging()
    .AddLog();

var app = builder.Build();

app
    .UseArchtectures()
    .UseGlobalErrorHandler()
    .MapCarter();

app.Run();