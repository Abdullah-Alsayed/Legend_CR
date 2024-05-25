using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using DicomApp.Helpers;

namespace DicomApp.Portal
{
    public static class DataSeeder
    {
        public static void Seed(ShippingDBContext dBContext)
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Editable = false,
                    Name = SystemConstants.Role.Admin,
                    CreationDate = System.DateTime.Now,
                    IsInternal = true,
                    IsDeleted = false
                },
                new Role
                {
                    Editable = false,
                    Name = SystemConstants.Role.Gamer,
                    CreationDate = System.DateTime.Now,
                    IsInternal = false,
                    IsDeleted = false
                }
            };
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
                    NameEN = "Reject",
                    NameAR = "ملغي",
                    StatusType = (int)StatusTypeEnum.Reject
                },
                new Status
                {
                    NameEN = "Complete",
                    NameAR = "مكتمل",
                    StatusType = (int)StatusTypeEnum.Complete
                }
            };

            if (!dBContext.Status.Any())
                dBContext.Status.AddRange(status);

            if (!dBContext.Role.Any())
            {
                dBContext.Role.AddRange(roles);
                dBContext.SaveChanges();
            }

            if (!dBContext.CommonUser.Any())
                dBContext.CommonUser.Add(
                    new CommonUser
                    {
                        Name = "System",
                        IsDeleted = false,
                        NationalId = "0000000000000000000000",
                        Email = "admin@admin.com",
                        Password = "123",
                        RoleId = dBContext
                            .Role.FirstOrDefault(x => x.Name == SystemConstants.Role.Admin)
                            .Id,
                        PhoneNumber = "01111111111",
                    }
                );

            dBContext.SaveChanges();
        }
    }
}
