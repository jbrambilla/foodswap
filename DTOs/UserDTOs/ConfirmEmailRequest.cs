namespace foodswap.DTOs.UserDTOs;

public class ConfirmEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string ConfirmationToken { get; set; } = string.Empty;
}