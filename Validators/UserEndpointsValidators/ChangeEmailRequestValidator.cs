using FluentValidation;
using foodswap.DTOs.UserDTOs;

namespace foodswap.Validators.UserEndpointsValidators;

public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
{
    public ChangeEmailRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty();
    }
}