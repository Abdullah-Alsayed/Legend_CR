using LegendCR.DAL.DB;
using LegendCR.DAL.Enums;
using Microsoft.AspNetCore.Identity;

namespace LegendCR.Core
{
    public static class DataSeeder
    {
        public static async Task SeedData(
            ApplicationDBContext context,
            RoleManager<Role> roleManager,
            UserManager<User> userManager
        )
        {
            await SeedRoles(context, roleManager);
            var systemUser = await GetUser(context, userManager);
            SeedSettings(context, systemUser);
            SeedStatuses(context, systemUser);
            context.SaveChanges();
        }

        private static async Task<string> GetUser(
            ApplicationDBContext context,
            UserManager<User> userManager
        )
        {
            var systemUser = Guid.NewGuid().ToString();
            if (!context.Users.Any(x => x.UserName == "System"))
            {
                var adminUser = new User()
                {
                    UserName = "System",
                    FirstName = "System",
                    LastName = "System",
                    Address = "System",
                    Email = "System",
                    EmailConfirmed = true,
                    CreateAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(adminUser, "P@ssword123");
                await userManager.AddToRoleAsync(adminUser, RoleEnum.SuperAdmin.ToString());
            }
            else
                systemUser = context.Users.FirstOrDefault(x => x.UserName == "System").Id;
            return systemUser;
        }

        private static void SeedStatuses(ApplicationDBContext context, string systemUser)
        {
            if (!context.Statuses.Any())
            {
                context.Statuses.Add(new Status { Name = "Complete", CreateBy = systemUser, });
            }
        }

        private static void SeedSettings(ApplicationDBContext context, string systemUser)
        {
            if (!context.Settings.Any())
            {
                context.Settings.Add(
                    new Settings
                    {
                        Address = "Dummy Data",
                        Email = "test@test.com",
                        FaceBook = "FaceBook.com",
                        Instagram = "Instagram.com",
                        MainColor = "#4990e2",
                        Logo = "Logo.png",
                        Phone = "011111111111",
                        Whatsapp = "whatsapp.com",
                        Youtube = "Youtube.com",
                        Title = "Dummy Data",
                        CreateBy = systemUser,
                    }
                );
            }
        }

        private static async Task SeedRoles(
            ApplicationDBContext context,
            RoleManager<Role> roleManager
        )
        {
            if (!context.Roles.Any())
            {
                var SuperAdmin = new Role
                {
                    Name = RoleEnum.SuperAdmin.ToString(),
                    NormalizedName = RoleEnum.SuperAdmin.ToString().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreateAt = DateTime.UtcNow,
                    IsMaster = true
                };

                await roleManager.CreateAsync(SuperAdmin);
                await roleManager.CreateAsync(
                    new Role
                    {
                        Name = RoleEnum.User.ToString(),
                        NormalizedName = RoleEnum.User.ToString().ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreateAt = DateTime.UtcNow,
                        IsMaster = false
                    }
                );
                await roleManager.CreateAsync(
                    new Role
                    {
                        Name = RoleEnum.Client.ToString(),
                        NormalizedName = RoleEnum.Client.ToString().ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        CreateAt = DateTime.UtcNow,
                        IsMaster = false
                    }
                );
            }
        }
    }
}
