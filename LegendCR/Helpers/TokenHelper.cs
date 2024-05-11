using LegendCR.Helpers;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.DAL.DB;
using LegendCR.BL.Services;

namespace LegendCR.API.Helpers
{
    public static class TokenHelper
    {
        public static bool Validate(string token, int userId, int roleId)
        {
            if (userId > 0 && roleId > 0)
            {
                string connectionString = Constants.connectionString;
                var context = new ApplicationDBContext(BaseService.GetDBContextConnectionOptions(connectionString));
                var userRequest = new UserRequest
                {
                    RoleID = roleId,
                    context = context,
                    UserDTO = new UserDTO
                    {
                        Id = userId
                    }
                };
                var userResponse = UserService.GetAllUsers(userRequest);
                if (userResponse.Success && userResponse.UserDTOs.Count > 0)
                {
                    var userToken = userResponse.UserDTOs[0].Token;
                    if (string.Equals(token, userToken))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
