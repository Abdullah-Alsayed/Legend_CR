using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Http;

namespace DicomApp.Helpers
{
    public class GeneralHelper
    {
        public static List<CommonUser> GetUsers(string RoleName, ShippingDBContext _context)
        {
            var UserList = _context
                .CommonUser.Where(p => !p.IsDeleted && p.Role.Name == RoleName)
                .ToList();
            return UserList;
        }
    }
}
