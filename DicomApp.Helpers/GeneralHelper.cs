using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DicomApp.Helpers
{
    public class GeneralHelper
    {
        public static List<CommonUser> GetUsers(int RoleID, ShippingDBContext _context)
        {
            var UserList = _context.CommonUser.Where(p => !p.IsDeleted && p.RoleId == RoleID).ToList();
            return UserList;
        }
    }
}
