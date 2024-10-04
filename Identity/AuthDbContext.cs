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

        builder.Entity<User>().Property(u => u.Name).HasMaxLength(200);

        builder.HasDefaultSchema("identity");
    }
}