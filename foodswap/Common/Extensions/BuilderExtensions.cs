using System.Text;
using Carter;
using FluentValidation;
using foodswap.Common.Api;
using foodswap.Common.Filters;
using foodswap.Common.Options;
using foodswap.Common.Services;
using foodswap.Data.Application;
using foodswap.Data.Identity;
using foodswap.Features;
using foodswap.Features.FoodFeatures.FoodDTOs;
using foodswap.Features.FoodFeatures.Validators;
using foodswap.Features.SwapperFeatures.DTOs;
using foodswap.Features.SwapperFeatures.Validators;
using foodswap.Features.TokenFeatures.TokenDTOs;
using foodswap.Features.TokenFeatures.Validators;
using foodswap.Features.UserFeatures.UserDTOs;
using foodswap.Features.UserFeatures.Validators;
using foodswap.tests.Features.SwapperFeatures.Models;
using foodswap.tests.Features.SwapperFeatures.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace foodswap.Common.Extensions;
public static class BuilderExtensions{

    public static WebApplicationBuilder AddArchtectures(this WebApplicationBuilder builder)
    {
        builder.Services.AddCarter();

        builder.Services.AddSingleton<TokenProvider>();

        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new StringToEnumConverter<EFoodCategory>());
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

        builder.Services
            .AddOptions<EmailSettingsOptions>()
            .Bind(builder.Configuration.GetSection(EmailSettingsOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<SentrySettingsOptions>()
            .Bind(builder.Configuration.GetSection(SentrySettingsOptions.SectionName))
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

            o.SchemaFilter<EnumSchemaFilter>();

            o.SwaggerDoc("v1", new OpenApiInfo{ Title = "FoodSwap API", Version = "v1" });
        });

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateOrUpdateFoodRequest>, CreateFoodRequestValidator>();
        
        builder.Services.AddScoped<IValidator<GetTokenRequest>, GetTokenRequestValidator>();
        builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordRequestValidator>();
        builder.Services.AddScoped<IValidator<ForgotPasswordRequest>, ForgotPasswordRequestValidator>();
        builder.Services.AddScoped<IValidator<ConfirmEmailRequest>, ConfirmEmailRequestValidator>();
        builder.Services.AddScoped<IValidator<UserResetPasswordRequest>, UserResetPasswordRequestValidator>();

        builder.Services.AddScoped<IValidator<CreateOrUpdateSwapperRequest>, CreateOrUpdateSwapperValidator>();
        builder.Services.AddScoped<IValidator<CreateFoodSwapRequest>, CreateFoodSwapRequestValidator>();
        builder.Services.AddScoped<IValidator<UpdateFoodSwapServingSizeRequest>, UpdateFoodSwapServingSizeRequestValidator>();

        builder.Services.AddScoped<EmailService>();

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
        var sentrySettingsOptions = builder.Configuration
            .GetSection(SentrySettingsOptions.SectionName)
            .Get<SentrySettingsOptions>();

        if (sentrySettingsOptions == null) {
            builder.Host.UseSerilog((ctx, lc) => lc
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/info-.txt",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    retainedFileCountLimit: 7
                ));
            return builder;
        }

        builder.Host.UseSerilog((ctx, lc) => lc
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/info-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Information,
                retainedFileCountLimit: 7
            )
            .WriteTo.Sentry(
                dsn: sentrySettingsOptions.Dsn,
                environment: builder.Environment.EnvironmentName,
                restrictedToMinimumLevel: LogEventLevel.Warning
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

        var jwtOptions = builder.Configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>();

        if (jwtOptions == null)
        {
            Log.Error("No JWT Settings configuration provided for JWT Auth initialization");
            throw new Exception("No JWT Settings configuration provided for JWT Auth initialization");
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

        builder.Services.AddDataProtection();

        return builder;
    }
}