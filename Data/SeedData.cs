using Hospitel_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace Hospitel_Project.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine(" Running SeedData.Initialize...");

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        
            var users = new List<(string Email, string Password)>
            {
                ("admin@example.com", "Admin@123"),
                ("user1@example.com", "User@123"),
                ("user2@example.com", "User@456"),
                ("manager@example.com", "Manager@789")
            };

            foreach (var (email, password) in users)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    Console.WriteLine($" Creating user: {email}");

                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"  User '{email}' created successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"  Failed to create user '{email}':");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"    - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"  User '{email}' already exists, skipping...");
                }
            }
        }
    }
}
