using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using foodswap.DTOs.FoodDTOs;
using foodswap.DTOs.UserDTOs;
using foodswap.Identity;
using foodswap.Validators;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Extensions;
public static class BuilderExtensions{
    public static WebApplicationBuilder AddArchtectures(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCarter();

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateFoodRequest>, CreateFoodRequestValidator>();
        builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();

        return builder;
    }

    public static WebApplicationBuilder AddHttpLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add("Authorization");
            logging.MediaTypeOptions.AddText("application/json");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
            logging.CombineLogs = true;
        });

        return builder;
    }

    public static WebApplicationBuilder AddLog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .Enrich.WithProperty("ApplicationName", "FoodSwap")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                builder.Configuration.GetConnectionString("AuditConnection"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                },
                restrictedToMinimumLevel: LogEventLevel.Information
            )
        );

        return builder;
    }

    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<AuthDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDataProtection();

        return builder;
    }
}