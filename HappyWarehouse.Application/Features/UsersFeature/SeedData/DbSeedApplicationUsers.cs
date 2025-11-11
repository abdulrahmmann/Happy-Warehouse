using HappyWarehouse.Application.Features.UsersFeature.Constants;
using HappyWarehouse.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HappyWarehouse.Application.Features.UsersFeature.SeedData
{
    public static class DbSeedApplicationUsers
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var roles = new[] { Roles.User, Roles.Admin, Roles.Management, Roles.Auditor };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var result = await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    });

                    if (result.Succeeded)
                        logger.Information($"Role '{roleName}' created.");
                    else
                        logger.Error($"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            var users = new List<(ApplicationUser User, string Password, string Role)>
            {
                (
                    new ApplicationUser
                    {
                        UserName = "admin@happywarehouse.com",
                        Email = "admin@happywarehouse.com",
                        FullName = "Happy Warehouse Admin",
                        PhoneNumber = "+962788362598",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false
                    },
                    "Admin@12345",
                    Roles.Admin
                ),
                (
                    new ApplicationUser
                    {
                        UserName = "user@happywarehouse.com",
                        Email = "user@happywarehouse.com",
                        FullName = "Happy Warehouse User",
                        PhoneNumber = "+96279354902",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false
                    },
                    "User@12345",
                    Roles.User
                ),
                (
                    new ApplicationUser
                    {
                        UserName = "management@happywarehouse.com",
                        Email = "management@happywarehouse.com",
                        FullName = "Happy Warehouse Management",
                        PhoneNumber = "+96277480029",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false
                    },
                    "Management@12345",
                    Roles.Management
                ),
                (
                    new ApplicationUser
                    {
                        UserName = "auditor@happywarehouse.com",
                        Email = "auditor@happywarehouse.com",
                        FullName = "Happy Warehouse Auditor",
                        PhoneNumber = "+962784739951",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        LockoutEnabled = false
                    },
                    "Auditor@12345",
                    Roles.Auditor
                )
            };
            
            foreach (var (user, password, role) in users)
            {
                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    var createResult = await userManager.CreateAsync(user, password);

                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                        logger.Information($"Created user: {user.Email} with role {role}");

                        // Optional: log all roles for verification
                        var assignedRoles = await userManager.GetRolesAsync(user);
                        logger.Information($"User '{user.Email}' roles: {string.Join(", ", assignedRoles)}");
                    }
                    else
                    {
                        logger.Error($"Failed to create user {user.Email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.Information($"User '{user.Email}' already exists.");

                    // Optional: ensure the user has the role
                    var rolesOfUser = await userManager.GetRolesAsync(existingUser);
                    if (!rolesOfUser.Contains(role))
                    {
                        await userManager.AddToRoleAsync(existingUser, role);
                        logger.Information($"Added missing role '{role}' to existing user '{user.Email}'.");
                    }
                }
            }

            logger.Information("Database seeding completed successfully!");
        }
    }
}
