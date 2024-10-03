using FluentValidation;
using foodswap.DTOs.TokenDTOs;
namespace foodswap.Validators;
public class GetTokenRequestValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}