using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace foodswap.Identity;
public class AuthDbContext : IdentityDbContext<User>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) 
    : base(options) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(u => u.Name)
            .HasColumnType("varchar(100)");

        builder.Entity<User>().Property(u => u.Surname)
            .HasColumnType("varchar(100)");

        builder.Entity<User>().Property(u => u.BirthDate)
            .HasColumnType("datetime");

        builder.HasDefaultSchema("identity");
    }
}