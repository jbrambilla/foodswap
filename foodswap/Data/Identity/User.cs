using Microsoft.AspNetCore.Identity;

namespace foodswap.Data.Identity;
public class User : IdentityUser
{
    public User(string name, string email) : base(email)
    {
        Name = name;
        Email = email;
    }

    public string Name { get; private set; } = string.Empty;
}