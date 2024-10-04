using System.Text;
using Carter;
using FluentValidation;
using foodswap;
using foodswap.DTOs.FoodDTOs;
using foodswap.DTOs.TokenDTOs;
using foodswap.DTOs.UserDTOs;
using foodswap.Identity;
using foodswap.Options;
using foodswap.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Extensions;
public static class BuilderExtensions{
    public static WebApplicationBuilder AddCarter(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();

        return builder;
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<ConnectionStringsOptions>()
            .Bind(builder.Configuration.GetSection(ConnectionStringsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<IdentitySeedOptions>()
            .Bind(builder.Configuration.GetSection(IdentitySeedOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(i => i.AdminUser != null && i.AdminUser.Email != null && i.AdminUser.Password != null, "AdminUser must have an email and password")
            .ValidateOnStart();

        builder.Services
            .AddOptions<JwtOptions>()
            .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }

    public static WebApplicationBuilder AddSwaggerWithAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
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

            o.SchemaFilter<ApiResponseSchemaFilter>();

            o.SwaggerDoc("v1", new OpenApiInfo{ Title = "FoodSwap API", Version = "v1" });
        });

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateFoodRequest>, CreateFoodRequestValidator>();
        builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        builder.Services.AddScoped<IValidator<GetTokenRequest>, GetTokenRequestValidator>();
        builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
        builder.Services.AddScoped<IValidator<ForgotPasswordRequest>, ForgotPasswordRequestValidator>();
        builder.Services.AddScoped<IValidator<ConfirmEmailRequest>, ConfirmEmailRequestValidator>();
        builder.Services.AddScoped<IValidator<ResetPasswordRequest>, ResetPasswordRequestValidator>();

        builder.Services.AddSingleton<TokenProvider>();
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
        var connectionStringsOptions = builder.Configuration
            .GetSection(ConnectionStringsOptions.SectionName)
            .Get<ConnectionStringsOptions>();

        if (connectionStringsOptions == null)
        {
            Log.Error("No ConnectionStrings configuration provided for Identity initialization");
            throw new Exception("No ConnectionStrings configuration provided for Identity initialization");
        }

        builder.Services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddDbContext<AuthDbContext>(options => 
            options.UseSqlServer(connectionStringsOptions.DefaultConnection));

        builder.Services.AddAuthorization(options => 
        {
            options.AddPolicy("AdminOrUser", policy => policy.RequireRole("admin", "user"));
            options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
        });
        
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