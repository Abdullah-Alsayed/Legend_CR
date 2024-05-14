using LegendCR.DAL.DB;

namespace LegendCR.Helpers
{
    public class GeneralHelper
    {
        public static List<User> GetUsers(int RoleID, ApplicationDBContext _context)
        {
            var UserList = _context.Users.Where(p => !p.IsDeleted).ToList();
            return UserList;
        }
    }
}
