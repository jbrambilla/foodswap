using FluentValidation;
using foodswap.Features.SwapperFeatures.DTOs;

namespace foodswap.Features.SwapperFeatures.Validators;

public class CreateOrUpdateSwapperValidator : AbstractValidator<CreateOrUpdateSwapperRequest>
{
    public CreateOrUpdateSwapperValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
    }
}