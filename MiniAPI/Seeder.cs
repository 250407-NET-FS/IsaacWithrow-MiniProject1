//This logic will run once.
//We create/use this AFTER we set up identity AND do a migration for that
using MiniAPI.Models;

using Microsoft.AspNetCore.Identity;
using MiniAPI.Data;

namespace MiniAPI;

//Im making this class static, I don't want anything to create a RolesInitializer object
public static class Seeder
{
    //This class will provide one static method to SeedRoles
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        const string email = "admin@admin.com";
        const string password = "Admin1234";
        const string FirstName = "Admin";
        const string LastName = "Admin";

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            if (!roleResult.Succeeded)
                throw new Exception($"Could not create Admin role: {string.Join("; ", roleResult.Errors.Select(e => e.Description))}");
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            if (!roleResult.Succeeded)
                throw new Exception($"Could not create User role: {string.Join("; ", roleResult.Errors.Select(e => e.Description))}");
        }



        if (await userManager.FindByEmailAsync(email) is null)
            {
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = email,
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = email,

                };

                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Seeding admin user failed: {errors}");
                }



                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

            }
    }
}