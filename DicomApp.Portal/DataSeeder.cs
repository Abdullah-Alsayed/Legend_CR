using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace DicomApp.Portal
{
    public static class DataSeeder
    {
        public static async Task Seed(ShippingDBContext dBContext)
        {
            try
            {
                await SeedStatus(dBContext);

                await SeedRoles(dBContext);

                await SeedUser(dBContext);

                await SeedCategorys(dBContext);

                await SeedCountries(dBContext);

                await dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task SeedCategorys(ShippingDBContext dBContext)
        {
            if (!await dBContext.Category.AnyAsync())
            {
                var category = new Category()
                {
                    NameAr = "اكشن",
                    NameEn = "Action",
                    CreatedAt = DateTime.Now,
                    Game = new List<Game>
                    {
                        new Game
                        {
                            NameAr = "ببجي",
                            NameEn = "Pubg",
                            ImgUrl = "pubg.png",
                            CreatedAt = DateTime.Now,
                        },
                        new Game
                        {
                            NameAr = "كلاش رويال",
                            NameEn = "Clash Royal",
                            ImgUrl = "ClashRoyal.png",
                            CreatedAt = DateTime.Now,
                        }
                    }
                };

                if (!await dBContext.Category.AnyAsync())
                    await dBContext.Category.AddAsync(category);
            }
        }

        private static async Task SeedUser(ShippingDBContext dBContext)
        {
            var role = await dBContext.Role.FirstOrDefaultAsync(x =>
                x.Name == SystemConstants.Role.SuperAdmin
            );
            if (!await dBContext.CommonUser.AnyAsync())
                await dBContext.CommonUser.AddAsync(
                    new CommonUser
                    {
                        Name = "System",
                        IsDeleted = false,
                        NationalId = "0000000000000000000000",
                        Email = "admin@admin.com",
                        Password = "123",
                        RoleId = role.Id,
                        PhoneNumber = "01111111111",
                    }
                );
        }

        private static async Task SeedRoles(ShippingDBContext dBContext)
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Editable = false,
                    Name = SystemConstants.Role.SuperAdmin,
                    CreationDate = System.DateTime.Now,
                    IsDeleted = false,
                },
                new Role
                {
                    Editable = false,
                    Name = SystemConstants.Role.Admin,
                    CreationDate = System.DateTime.Now,
                    IsDeleted = false
                },
                new Role
                {
                    Editable = false,
                    Name = SystemConstants.Role.Gamer,
                    CreationDate = System.DateTime.Now,
                    IsDeleted = false
                }
            };

            if (!await dBContext.Role.AnyAsync())
            {
                await dBContext.Role.AddRangeAsync(roles);
                await dBContext.SaveChangesAsync();
            }
        }

        private static async Task SeedStatus(ShippingDBContext dBContext)
        {
            var status = new List<Status>()
            {
                new Status
                {
                    NameEN = "In Progress",
                    NameAR = "قيد العمل",
                    StatusType = (int)StatusTypeEnum.InProgress
                },
                new Status
                {
                    NameEN = "Accept",
                    NameAR = "مقبول",
                    StatusType = (int)StatusTypeEnum.Accept
                },
                new Status
                {
                    NameEN = "Published",
                    NameAR = "منشور",
                    StatusType = (int)StatusTypeEnum.Published
                },
                new Status
                {
                    NameEN = "Pending",
                    NameAR = "قيد الانتظار",
                    StatusType = (int)StatusTypeEnum.Pending
                },
                new Status
                {
                    NameEN = "Reject",
                    NameAR = "مرفوض",
                    StatusType = (int)StatusTypeEnum.Reject
                },
                new Status
                {
                    NameEN = "Sold",
                    NameAR = "مباع",
                    StatusType = (int)StatusTypeEnum.Sold
                }
            };

            if (!await dBContext.Status.AnyAsync())
                await dBContext.Status.AddRangeAsync(status);
        }

        private static async Task SeedCountries(ShippingDBContext dBContext)
        {
            if (!await dBContext.Countries.AnyAsync())
            {
                string jsonFilePath = "Jsons/countries.json";
                string jsonString = await File.ReadAllTextAsync(jsonFilePath);
                List<Country> countries = JsonSerializer.Deserialize<List<Country>>(jsonString);
                await dBContext.Countries.AddRangeAsync(countries);
            }
        }
    }
}
