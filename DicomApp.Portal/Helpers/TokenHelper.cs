using DicomApp.DAL.DB;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.Portal;
using Microsoft.Extensions.Configuration;

namespace DicomApp.API.Helpers
{
    public static class TokenHelper
    {
        public static bool Validate(string token, int userId, int roleId)
        {
            if (userId > 0 && roleId > 0)
            {
                string connectionString = Startup.Configuration["ConnectionStrings:GTSportsDBEntities"];
                var context = new ShippingDBContext(BaseService.GetDBContextConnectionOptions(connectionString));
                var userRequest = new UserRequest
                {
                    RoleID = roleId,
                    context=context,
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
