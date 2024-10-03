using FluentValidation;
using foodswap.DTOs.UserDTOs;
namespace foodswap.Validators;
public class GetTokenRequestValidator : AbstractValidator<GetTokenRequest>
{
    public GetTokenRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}