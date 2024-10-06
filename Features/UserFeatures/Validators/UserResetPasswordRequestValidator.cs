using FluentValidation;
using foodswap.Features.UserFeatures.UserDTOs;

namespace foodswap.Features.UserFeatures.Validators;

public class UserResetPasswordRequestValidator : AbstractValidator<UserResetPasswordRequest>
{
    public UserResetPasswordRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}