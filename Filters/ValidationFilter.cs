using Carter.ModelBinding;
using FluentValidation;
namespace foodswap.Filters;
public class ValidatorFilter<TRequest> : IEndpointFilter where TRequest : class
{
    private readonly IValidator<TRequest> _validator;
    public ValidatorFilter(IValidator<TRequest> validator)
    {
        _validator = validator;
    }
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments
            .FirstOrDefault(x => x is TRequest)
            as TRequest;
        var result = await _validator.ValidateAsync(request!);
        if (!result.IsValid) return Results.BadRequest(new ApiResponse<object>(false, "Validation Error", null!, result.Errors.Select(x => x.ErrorMessage).ToList()));
        return await next(context);
    }
}