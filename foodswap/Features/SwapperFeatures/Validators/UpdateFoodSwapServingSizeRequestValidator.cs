using FluentValidation;
using foodswap.tests.Features.SwapperFeatures.Models;

namespace foodswap.tests.Features.SwapperFeatures.Validators;

public class UpdateFoodSwapServingSizeRequestValidator : AbstractValidator<UpdateFoodSwapServingSizeRequest>
{
    public UpdateFoodSwapServingSizeRequestValidator()
    {
        RuleFor(x => x.ServingSize).GreaterThanOrEqualTo(0);
    }
}