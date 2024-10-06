using FluentValidation;
using foodswap.DTOs.UserDTOs;

namespace foodswap.Validators.UserEndpointsValidators;

public class RequestEmailChangeRequestValidator : AbstractValidator<RequestEmailChangeRequest>
{
    public RequestEmailChangeRequestValidator()
    {
        RuleFor(x => x.NewEmail).NotEmpty().EmailAddress();
    }
}