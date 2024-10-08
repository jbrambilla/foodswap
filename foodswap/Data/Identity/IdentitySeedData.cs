using foodswap.Common.Options;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace foodswap.Data.Identity;

public static class IdentitySeedData
{

    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        var identitySeedOptions = configuration
            .GetSection(IdentitySeedOptions.SectionName)
            .Get<IdentitySeedOptions>();

        if (identitySeedOptions == null)
        {
            Log.Error("No IdentitySeed configuration provided");
            return;
        }

        foreach (var role in identitySeedOptions.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    public static async Task SeedAdminUser(UserManager<User> userManager, IConfiguration configuration)
    {
        var identitySeedOptions = configuration
            .GetSection(IdentitySeedOptions.SectionName)
            .Get<IdentitySeedOptions>();

        if (identitySeedOptions == null)
        {
            Log.Error("No IdentitySeed configuration provided");
            return;
        }

        var adminUser = await userManager.FindByEmailAsync(identitySeedOptions.AdminUser.Email);
        if (adminUser == null)
        {
            var user = new User(
                identitySeedOptions.AdminUser.Name,
                identitySeedOptions.AdminUser.Email)
            { 
                EmailConfirmed = true 
            };
            
            var result = await userManager.CreateAsync(user, identitySeedOptions.AdminUser.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, identitySeedOptions.AdminUser.Role);
            }
        }
    }
}