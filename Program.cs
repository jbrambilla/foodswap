using Carter;
using Extensions;
using foodswap.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddArchtectures()
    .AddSwaggerWithAuth()
    .AddServices()
    .AddHttpLogging()
    .AddLog()
    .AddIdentity();
    
var app = builder.Build();

app
    .UseArchtectures()
    .UseGlobalErrorHandler()
    .MapCarter();

app.Run();