using System.Text;
using Carter;
using FluentValidation;
using foodswap.DTOs.FoodDTOs;
using foodswap.DTOs.TokenDTOs;
using foodswap.DTOs.UserDTOs;
using foodswap.Identity;
using foodswap.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Extensions;
public static class BuilderExtensions{
    public static WebApplicationBuilder AddArchtectures(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<TokenProvider>();

        builder.Services.AddCarter();

        return builder;
    }

    public static WebApplicationBuilder AddSwaggerWithAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            o.AddSecurityRequirement(securityRequirement);
        });

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateFoodRequest>, CreateFoodRequestValidator>();
        builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        builder.Services.AddScoped<IValidator<GetTokenRequest>, GetTokenRequestValidator>();

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
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<AuthDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddDataProtection();

        return builder;
    }
}