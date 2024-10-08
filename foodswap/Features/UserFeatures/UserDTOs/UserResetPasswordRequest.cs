namespace foodswap.Features.UserFeatures.UserDTOs;

public class UserResetPasswordRequest
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public  string NewPassword { get; set; } = string.Empty;
}