using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Http;

namespace DicomApp.Helpers
{
    public static class GeneralHelper
    {
        public static List<CommonUser> GetUsers(string RoleName, ShippingDBContext _context)
        {
            var UserList = _context
                .CommonUser.Where(p => !p.IsDeleted && p.Role.Name == RoleName)
                .ToList();
            return UserList;
        }

        public static string RemoveWhiteSpace(this string value)
        {
            string result = Regex.Replace(value, @"\s+", "");
            return result;
        }
    }
}
