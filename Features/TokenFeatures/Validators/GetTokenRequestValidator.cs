using FluentValidation;
using foodswap.Features.TokenFeatures.TokenDTOs;

namespace foodswap.Features.TokenFeatures.Validators;
public class GetTokenRequestValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}