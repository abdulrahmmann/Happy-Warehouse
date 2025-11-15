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
            // 5 Admin
            (
                new ApplicationUser
                {
                    UserName = "admin@happywarehouse",
                    Email = "admin@happywarehouse.com",
                    FullName = "Admin Happy Warehouse",
                    PhoneNumber = "+962783363098",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Admin@happywarehouse+962783363098",
                Roles.Admin
            ),
            (
                new ApplicationUser
                {
                    UserName = "ahmednaser@admin",
                    Email = "ahmednaser@happywarehouse.com",
                    FullName = "Ahmed Naser",
                    PhoneNumber = "+962788362001",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Ahmednaser@admin+962788362001",
                Roles.Admin
            ),
            (
                new ApplicationUser
                {
                    UserName = "leenaomar@admin",
                    Email = "leenaomar@happywarehouse.com",
                    FullName = "Leena Omar",
                    PhoneNumber = "+962788362002",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Leenaomar@admin+962788362002",
                Roles.Admin
            ),
            (
                new ApplicationUser
                {
                    UserName = "youssefsaleh@admin",
                    Email = "youssefsaleh@happywarehouse.com",
                    FullName = "Youssef Saleh",
                    PhoneNumber = "+962788362003",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Youssefsaleh@admin+962788362003",
                Roles.Admin
            ),
            (
                new ApplicationUser
                {
                    UserName = "mariamkhalid@admin",
                    Email = "mariamkhalid@happywarehouse.com",
                    FullName = "Mariam Khalid",
                    PhoneNumber = "+962788362004",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Mariamkhalid@admin+962788362004",
                Roles.Admin
            ),
            (
                new ApplicationUser
                {
                    UserName = "tariqfahad@admin",
                    Email = "tariqfahad@happywarehouse.com",
                    FullName = "Tariq Fahad",
                    PhoneNumber = "+962788362005",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Tariqfahad@admin+962788362005",
                Roles.Admin
            ),

            // 5 Auditor
            (
                new ApplicationUser
                {
                    UserName = "samarhassan@auditor",
                    Email = "samarhassan@happywarehouse.com",
                    FullName = "Samar Hassan",
                    PhoneNumber = "+962788362006",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Samarhassan@auditor+962788362006",
                Roles.Auditor
            ),
            (
                new ApplicationUser
                {
                    UserName = "fadiabdel@auditor",
                    Email = "fadiabdel@happywarehouse.com",
                    FullName = "Fadi Abdel",
                    PhoneNumber = "+962788362007",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Fadiabdel@auditor+962788362007",
                Roles.Auditor
            ),
            (
                new ApplicationUser
                {
                    UserName = "noorhadi@auditor",
                    Email = "noorhadi@happywarehouse.com",
                    FullName = "Noor Hadi",
                    PhoneNumber = "+962788362008",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Noorhadi@auditor+962788362008",
                Roles.Auditor
            ),
            (
                new ApplicationUser
                {
                    UserName = "reemmahmoud@auditor",
                    Email = "reemmahmoud@happywarehouse.com",
                    FullName = "Reem Mahmoud",
                    PhoneNumber = "+962788362009",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Reemmahmoud@auditor+962788362009",
                Roles.Auditor
            ),
            (
                new ApplicationUser
                {
                    UserName = "omarjalal@auditor",
                    Email = "omarjalal@happywarehouse.com",
                    FullName = "Omar Jalal",
                    PhoneNumber = "+962788362010",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Omarjalal@auditor+962788362010",
                Roles.Auditor
            ),

            // 5 Management
            (
                new ApplicationUser
                {
                    UserName = "dinaalsharif@management",
                    Email = "dinaalsharif@happywarehouse.com",
                    FullName = "Dina Al Sharif",
                    PhoneNumber = "+962788362011",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Dinaalsharif@management+962788362011",
                Roles.Management
            ),
            (
                new ApplicationUser
                {
                    UserName = "khaledjawad@management",
                    Email = "khaledjawad@happywarehouse.com",
                    FullName = "Khaled Jawad",
                    PhoneNumber = "+962788362012",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Khaledjawad@management+962788362012",
                Roles.Management
            ),
            (
                new ApplicationUser
                {
                    UserName = "asmaaabbas@management",
                    Email = "asmaaabbas@happywarehouse.com",
                    FullName = "Asmaa Abbas",
                    PhoneNumber = "+962788362013",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Asmaaabbas@management+962788362013",
                Roles.Management
            ),
            (
                new ApplicationUser
                {
                    UserName = "zainalsaleh@management",
                    Email = "zainalsaleh@happywarehouse.com",
                    FullName = "Zain Al Saleh",
                    PhoneNumber = "+962788362014",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Zainalsaleh@management+962788362014",
                Roles.Management
            ),
            (
                new ApplicationUser
                {
                    UserName = "laylamahmoud@management",
                    Email = "laylamahmoud@happywarehouse.com",
                    FullName = "Layla Mahmoud",
                    PhoneNumber = "+962788362015",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Laylamahmoud@management+962788362015",
                Roles.Management
            ),

            // 15 Users
            (
                new ApplicationUser
                {
                    UserName = "husseinali@user",
                    Email = "husseinali@happywarehouse.com",
                    FullName = "Hussein Ali",
                    PhoneNumber = "+962788362016",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Husseinali@user+962788362016",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "reemsaeed@user",
                    Email = "reemsaeed@happywarehouse.com",
                    FullName = "Reem Saeed",
                    PhoneNumber = "+962788362017",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Reemsaeed@user+962788362017",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "fadiyousef@user",
                    Email = "fadiyousef@happywarehouse.com",
                    FullName = "Fadi Yousef",
                    PhoneNumber = "+962788362018",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Fadiyousef@user+962788362018",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "samarhadi@user",
                    Email = "samarhadi@happywarehouse.com",
                    FullName = "Samar Hadi",
                    PhoneNumber = "+962788362019",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Samarhadi@user+962788362019",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "mohannadkhalil@user",
                    Email = "mohannadkhalil@happywarehouse.com",
                    FullName = "Mohannad Khalil",
                    PhoneNumber = "+962788362020",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Mohannadkhalil@user+962788362020",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "laylabakr@user",
                    Email = "laylabakr@happywarehouse.com",
                    FullName = "Layla Bakr",
                    PhoneNumber = "+962788362021",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Laylabakr@user+962788362021",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "ahmedsami@user",
                    Email = "ahmedsami@happywarehouse.com",
                    FullName = "Ahmed Sami",
                    PhoneNumber = "+962788362022",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Ahmedsami@user+962788362022",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "yasminomar@user",
                    Email = "yasminomar@happywarehouse.com",
                    FullName = "Yasmin Omar",
                    PhoneNumber = "+962788362023",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Yasminomar@user+962788362023",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "fahadsaleh@user",
                    Email = "fahadsaleh@happywarehouse.com",
                    FullName = "Fahad Saleh",
                    PhoneNumber = "+962788362024",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Fahadsaleh@user+962788362024",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "reemkhalid@user",
                    Email = "reemkhalid@happywarehouse.com",
                    FullName = "Reem Khalid",
                    PhoneNumber = "+962788362025",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Reemkhalid@user+962788362025",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "mohamedfadi@user",
                    Email = "mohamedfadi@happywarehouse.com",
                    FullName = "Mohamed Fadi",
                    PhoneNumber = "+962788362026",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Mohamedfadi@user+962788362026",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "linaahmad@user",
                    Email = "linaahmad@happywarehouse.com",
                    FullName = "Lina Ahmad",
                    PhoneNumber = "+962788362027",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Linaahmad@user+962788362027",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "samirhassan@user",
                    Email = "samirhassan@happywarehouse.com",
                    FullName = "Samir Hassan",
                    PhoneNumber = "+962788362028",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Samirhassan@user+962788362028",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "fadiyahya@user",
                    Email = "fadiyahya@happywarehouse.com",
                    FullName = "Fadi Yahya",
                    PhoneNumber = "+962788362029",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Fadiyahya@user+962788362029",
                Roles.User
            ),
            (
                new ApplicationUser
                {
                    UserName = "mahmoudsaleh@user",
                    Email = "mahmoudsaleh@happywarehouse.com",
                    FullName = "Mahmoud Saleh",
                    PhoneNumber = "+962788362030",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                },
                "Mahmoudsaleh@user+962788362030",
                Roles.User
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
