using LegendCR.DAL.DB;

namespace LegendCR.Helpers
{
    public class GeneralHelper
    {
        public static List<CommonUser> GetUsers(int RoleID, ApplicationDBContext _context)
        {
            var UserList = _context.CommonUser.Where(p => !p.IsDeleted && p.RoleId == RoleID).ToList();
            return UserList;
        }
    }
}
