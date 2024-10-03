using foodswap.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace foodswap.Endpoints;

public static class IdentitySeedData
{

    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        var seeded = configuration.GetValue<bool>("Identity:Seeded:Roles");
        if (seeded){
            Log.Information("Roles already seeded.");
            return;
        }

        var roles = configuration.GetSection("Identity:Roles").Get<List<string>>();
        if (roles is null || roles.Count == 0)
        {
            Log.Error("No roles defined in the configuration file.");
            return;
        }

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        configuration["Identity:Seeded:Roles"] = "true";
    }
    public static async Task SeedAdminUser(UserManager<User> userManager, IConfiguration configuration)
    {
        var seeded = configuration.GetValue<bool>("Identity:Seeded:AdminUser");
        if (seeded){
            Log.Information("Admin user already seeded.");
            return;
        }

        var adminUserName = configuration["Identity:AdminUser:Name"];
        var adminUserSurname = configuration["Identity:AdminUser:Surname"];
        var adminUserPhoneNumber = configuration["Identity:AdminUser:PhoneNumber"];
        var adminUserBirthDate = configuration.GetValue<DateTime>("Identity:AdminUser:BirthDate");
        var adminUserEmail = configuration["Identity:AdminUser:Email"];
        var adminUserPassword = configuration["Identity:AdminUser:Password"];
        
        if (string.IsNullOrEmpty(adminUserEmail)     
            || string.IsNullOrEmpty(adminUserPassword) 
            || string.IsNullOrEmpty(adminUserName) 
            || string.IsNullOrEmpty(adminUserSurname)
            || string.IsNullOrEmpty(adminUserPhoneNumber)
            || adminUserBirthDate == default) {
                Log.Error("Invalid configuration for admin user. Please check the configuration file.");
                return;
            }

        var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
        if (adminUser == null)
        {
            var user = new User(adminUserName, adminUserSurname, adminUserEmail, adminUserPhoneNumber, adminUserBirthDate) 
            { 
                PhoneNumberConfirmed = true, 
                EmailConfirmed = true 
            };
            
            var result = await userManager.CreateAsync(user, adminUserPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
                configuration["Identity:Seeded:AdminUser"] = "true";
            }
        }
    }
}