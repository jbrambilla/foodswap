namespace foodswap.DTOs.UserDTOs;
public class GetTokenRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}