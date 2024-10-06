using FluentValidation;
using foodswap.DTOs.UserDTOs;

namespace foodswap.Validators.UserEndpointsValidators;

public class UserResetPasswordRequestValidator : AbstractValidator<UserResetPasswordRequest>
{
    public UserResetPasswordRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}