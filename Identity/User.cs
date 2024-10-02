using Microsoft.AspNetCore.Identity;

namespace foodswap.Identity;
public class User : IdentityUser
{
    public User(string name, string surname, string email, string phoneNumber, DateTime birthDate) : base(email)
    {
        Name = name;
        Surname = surname;
        PhoneNumber = phoneNumber;
        Email = email;
        BirthDate = birthDate;
    }

    public string Name { get; private set; } = string.Empty;
    public string Surname { get; private set; } = string.Empty;
    public DateTime BirthDate { get; private set; }
}