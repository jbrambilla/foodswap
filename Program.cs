using Carter;
using Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchtectures()
    .AddServices()
    .AddLog();

var app = builder.Build();

app
    .UseArchtectures()
    .UseGlobalErrorHandler()
    .UseLogRequestHandler()
    .MapCarter();

app.Run();