namespace foodswap.Features.TokenFeatures.TokenDTOs;

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}