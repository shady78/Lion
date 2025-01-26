using Lion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Infrastructure.Data;
public static class SeedData
{
    public static async Task Initialize(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<Role> roleManager)
    {
        context.Database.Migrate();

        // Seed Roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new Role("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new Role("User"));
        }

        // Seed Permissions
        if (!context.Permissions.Any())
        {
            var permissions = new List<Permission>
            {
                new Permission { Name = "CreateProduct", Description = "Can create products" },
                new Permission { Name = "EditProduct", Description = "Can edit products" },
                new Permission { Name = "DeleteProduct", Description = "Can delete products" },
                new Permission { Name = "ViewProduct", Description = "Can view products" }
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        var adminEmail = "admin@example.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                CompanyName = "Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123*");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
