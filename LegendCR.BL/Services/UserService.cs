using System.Net;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.DAL.DB;
using LegendCR.Helpers;

namespace LegendCR.BL.Services
{
    public class UserService : BaseService
    {
        public static UserResponse GetAllUsers(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Users.Where(c => !c.IsDeleted)
                            .Select(c => new UserDTO { Name = c.UserName, Id = c.Id });

                        if (request.applyFilter)
                            query = ApplyFilter(query, request.UserDTO);

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.UserDTOs = query.ToList();
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse GetLastUser(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Users.Where(c => !c.IsDeleted)
                            .Select(c => new UserDTO { Id = c.Id, Name = c.UserName })
                            .LastOrDefault();

                        res.TotalCount = 1;
                        res.UserDTO = query;
                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse GetUser(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Users.Where(c => !c.IsDeleted && c.Id == request.UserDTO.Id)
                            .Select(c => new UserDTO { Id = c.Id, Name = c.UserName, });

                        res.UserDTO = query.FirstOrDefault();

                        res.Message = HttpStatusCode.OK.ToString();
                        res.Success = true;
                        res.StatusCode = HttpStatusCode.OK;
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse Login(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var model = request.UserDTO;

                        var user = request
                            .context.Users.Where(c => !c.IsDeleted && c.Email == model.Email)
                            .Select(c => new { user = c })
                            .FirstOrDefault();

                        if (user != null)
                        {
                            //update user last login
                            //user.user.LastLoginDate = DateTime.Now;
                            //user.user.IsLoggedIn = true;
                            //request._context.SaveChanges();

                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;

                            res.UserDTOs = new List<UserDTO>
                            {
                                new UserDTO
                                {
                                    Id = user.user.Id,
                                    Name = user.user.UserName,
                                    PhoneNumber = user.user.PhoneNumber,
                                }
                            };
                        }
                        else
                        {
                            res.Message = "Invalid username or password";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse DeleteUser(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var model = request.UserDTO;
                        var user = request.context.Users.FirstOrDefault(c =>
                            !c.IsDeleted && c.Id == model.Id
                        );
                        if (user != null)
                        {
                            //update user IsDeleted
                            user.IsDeleted = true;
                            user.DeletedAt = DateTime.Now;
                            user.DeletedBy = req.UserID;
                            request.context.SaveChanges();

                            res.Message = "User deleted successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid user delete";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse EditUser(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var user = request.context.Users.Find(request.UserDTO.Id);

                        if (user != null)
                        {
                            var UserExist = request.context.Users.Any(m =>
                                m.Email == request.UserDTO.Email
                                && m.Email != user.Email
                                && m.IsDeleted == false
                            );
                            if (!UserExist)
                            {
                                //user.ModificationDate = DateTime.Now;
                                //user.ModifiedBy = request.UserID;
                                //user.RoleId = request.UserDTO.RoleID;
                                //user.Name = request.UserDTO.Name;
                                //user.Email = request.UserDTO.Email;
                                //user.PhoneNumber = request.UserDTO.PhoneNumber;
                                //user.AreaId = request.UserDTO.AreaId;
                                //user.ZoneId = request.UserDTO.ZoneId;
                                //user.Address = request.UserDTO.Address;
                                //user.FullName = request.UserDTO.FullName;
                                //user.AddressDetails = request.UserDTO.AddressDetails;
                                //user.ProductType = request.UserDTO.ProductType;
                                //user.Averageorders = request.UserDTO.Averageorders;
                                //user.PageName = request.UserDTO.PageName;
                                //user.VisaPayment = request.UserDTO.VisaPayment;
                                //user.VisaPayment = request.UserDTO.VisaPayment;
                                //user.Landmark = request.UserDTO.Landmark;
                                //user.LocationUrl = request.UserDTO.LocationUrl;
                                //user.Floor = request.UserDTO.Floor;
                                //user.Apartment = request.UserDTO.Apartment;
                                //user.IsLoggedIn = request.UserDTO.IsLoggedIn;
                                //user.Name = request.UserDTO.Name;
                                //user.InstPayAccountName = request.UserDTO.InstaPayAccountName;
                                //user.IbanNumber = request.UserDTO.IBANNumber;
                                //user.Bank = request.UserDTO.Bank;
                                //user.VodafoneCashNumber = request.UserDTO.VodafoneCashNumber;
                                //user.AccountName = request.UserDTO.AccountName;
                                //user.AccountNumber = request.UserDTO.AccountNumber;
                                //user.ImgUrl = request.UserDTO.ImgUrl;

                                request.context.SaveChanges();

                                res.Message = "User updated successfully";
                                res.Success = true;
                                res.StatusCode = HttpStatusCode.OK;
                            }
                            else
                            {
                                res.Message = "Email already exist";
                                res.Success = false;
                                res.StatusCode = HttpStatusCode.OK;
                            }
                        }
                        else
                        {
                            res.Message = "Invalid update user";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse ChangePassword(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var user = request.context.Users.Find(request.UserDTO.Id);
                        if (user != null)
                        {
                            user.ModifyAt = DateTime.Now;
                            user.ModifyBy = request.UserID;

                            request.context.SaveChanges();

                            res.Message = "Password updated successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Invalid update Password";
                            res.Success = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        public static UserResponse AddUser(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var UserExist = request.context.Users.Any(m =>
                            m.Email == request.UserDTO.Email && m.IsDeleted == false
                        );
                        if (!UserExist)
                        {
                            var User = new User();

                            request.context.Users.Add(User);
                            request.context.SaveChanges();

                            // Add User Account for Vendor/Courier


                            request.context.SaveChanges();

                            res.Message = "New user added successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            res.Message = "Email already exist";
                            res.Success = false;
                            res.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        res.Message = ex.Message;
                        res.Success = false;
                        LogHelper.LogException(ex.Message, ex.StackTrace);
                    }
                    return res;
                }
            );
            return res;
        }

        private static IQueryable<UserDTO> ApplyFilter(IQueryable<UserDTO> query, UserDTO filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(c =>
                    (c.Name.Contains(filter.Search))
                    || (c.PhoneNumber.Contains(filter.Search))
                    || (c.Email.Contains(filter.Search))
                    || (c.RoleName.Contains(filter.Search))
                );

            if (!string.IsNullOrWhiteSpace(filter.Id))
                query = query.Where(c => c.Id == filter.Id);

            if (filter.RoleID == (long)EnumRole.Vendor)
                query = query.Where(c => c.RoleID == filter.RoleID);

            if (filter.RoleID == (int)EnumRole.Employee)
                query = query.Where(u =>
                    u.RoleID != (int)EnumRole.Vendor
                    && u.RoleID != (int)EnumRole.Customer
                    && !u.IsDeleted
                );

            if (
                filter.RoleID > 0
                && filter.RoleID != (int)EnumRole.Vendor
                && filter.RoleID != (int)EnumRole.Employee
            )
                query = query.Where(c => c.RoleID == filter.RoleID);

            if (filter.StaffOnly)
                query = query.Where(c =>
                    c.RoleID != (int)EnumRole.Vendor && c.RoleID != (int)EnumRole.Customer
                );

            //if (filter.IsStaff == true)
            //    query = query.Where(c => c.RoleID != 4 && c.RoleID != 3);

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(c => c.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                query = query.Where(c =>
                    c.PhoneNumber.Trim()
                        .IndexOf(filter.PhoneNumber.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                );

            if (filter.HasCourierShipments)
                query = query.Where(c =>
                    c.CourierShipments != null && c.CourierShipments.Count() > 0
                );

            return query;
        }
    }
}
