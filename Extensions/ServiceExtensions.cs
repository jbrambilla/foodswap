using Carter;
using FluentValidation;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCarter();

        services.AddScoped<IValidator<FoodRequest>, FoodRequestValidator>();

        return services;
    }
}