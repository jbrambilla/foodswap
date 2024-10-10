using System.Security.Claims;
using System.Text;
using foodswap.Common.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace foodswap.Data.Identity;
public sealed class TokenProvider(IConfiguration configuration)
{
    public TokenProviderResponse Create(User user, string[] roles)
    {
        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>();

        if (jwtOptions == null)
        {
            Log.Error("No Jwt configuration provided");
            throw new Exception("No Jwt configuration provided");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("email_confirmed", user.EmailConfirmed.ToString().ToLower()),
                new Claim("roles", string.Join(",", roles))
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpiryMinutes),
            SigningCredentials = credentials,
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return new TokenProviderResponse{ Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(handler.TokenLifetimeInMinutes) };
    }
}

public class TokenProviderResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}