using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.GenrateAvatar;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace DicomApp.BL.Services
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
                            .context.CommonUser.Where(c => !c.IsDeleted)
                            .Select(c => new UserDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                NationalId = c.NationalId,
                                Email = c.Email,
                                RoleID = c.RoleId,
                                Gender = c.Gender,
                                Age = c.Age,
                                RoleName = c.Role.Name,
                                LastLoginDate = c.LastLoginDate,
                                PhoneNumber = c.PhoneNumber,
                                Password = c.Password,
                                ConfirmPassword = c.Password,
                                IsLoggedIn = c.IsLoggedIn,
                                Address = c.Address,
                                AreaId = c.AreaId,
                                Lat = c.Lat,
                                Lng = c.Lng,
                                ModificationDate = c.ModificationDate,
                                Code = c.Code,
                                AddressDetails = c.AddressDetails,
                                FullName = c.FullName,
                                PageName = c.PageName,
                                Averageorders = c.Averageorders,
                                ProductType = c.ProductType,
                                VisaPayment = c.VisaPayment,
                                CreationDate = c.CreationDate,
                                Apartment = c.Apartment,
                                Floor = c.Floor,
                                Landmark = c.Landmark,
                                LocationUrl = c.LocationUrl,
                                WalletNumber = c.WalletNumber,
                                AccountName = c.AccountName,
                                AccountNumber = c.AccountNumber,
                                Bank = c.Bank,
                                Language = c.Language,
                                IBANNumber = c.IbanNumber,
                                ImgUrl = c.ImgUrl,
                                InstaPayAccountName = c.InstaPayAccountName,
                                ZoneId = c.ZoneId,
                                IsDeleted = c.IsDeleted,
                                ZoneName = c.ZoneId.HasValue
                                    ? request
                                        .context.Zone.FirstOrDefault(u => u.Id == c.ZoneId)
                                        .NameAr
                                    : "",
                                AreaName = c.AreaId.HasValue
                                    ? request
                                        .context.City.FirstOrDefault(a => a.Id == c.AreaId)
                                        .CityNameAr
                                    : "",
                                Country =
                                    c.Country != null
                                        ? new CountryDTO
                                        {
                                            CountryCode = c.Country.CountryCode,
                                            CountryId = c.Country.CountryId,
                                            FlagUrl = c.Country.FlagUrl,
                                            NameAr = c.Country.NameAr,
                                            NameEn = c.Country.NameEn
                                        }
                                        : null,
                                TelegramUserName = c.TelegramUserName,
                            });

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
                            .context.CommonUser.Where(c => !c.IsDeleted)
                            .Select(c => new UserDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                NationalId = c.NationalId,
                                Email = c.Email,
                                RoleID = c.RoleId,
                                RoleName = c.Role.Name,
                                Gender = c.Gender,
                                Age = c.Age,
                                LastLoginDate = c.LastLoginDate,
                                PhoneNumber = c.PhoneNumber,
                                Password = c.Password,
                                ConfirmPassword = c.Password,
                                IsLoggedIn = c.IsLoggedIn,
                                Address = c.Address,
                                AreaId = c.AreaId,
                                Lat = c.Lat,
                                Lng = c.Lng,
                                Language = c.Language,
                                ModificationDate = c.ModificationDate,
                                Code = c.Code,
                                AddressDetails = c.AddressDetails,
                                FullName = c.FullName,
                                PageName = c.PageName,
                                Averageorders = c.Averageorders,
                                ProductType = c.ProductType,
                                VisaPayment = c.VisaPayment,
                                CreationDate = c.CreationDate,
                                Apartment = c.Apartment,
                                Floor = c.Floor,
                                Landmark = c.Landmark,
                                LocationUrl = c.LocationUrl,
                                WalletNumber = c.WalletNumber,
                                AccountName = c.AccountName,
                                AccountNumber = c.AccountNumber,
                                Bank = c.Bank,
                                IBANNumber = c.IbanNumber,
                                ImgUrl = c.ImgUrl,
                                InstaPayAccountName = c.InstaPayAccountName,
                                ZoneId = c.ZoneId,
                                IsDeleted = c.IsDeleted,
                                TelegramUserName = c.TelegramUserName,
                            })
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
                            .context.CommonUser.Where(c =>
                                !c.IsDeleted && c.Id == request.UserDTO.Id
                            )
                            .Select(c => new UserDTO
                            {
                                Id = c.Id,
                                Name = c.Name,
                                NationalId = c.NationalId,
                                Email = c.Email,
                                RoleID = c.RoleId,
                                RoleName = c.Role.Name,
                                Gender = c.Gender,
                                Age = c.Age,
                                CountryId = c.CountryId,
                                LastLoginDate = c.LastLoginDate,
                                PhoneNumber = c.PhoneNumber,
                                Password = c.Password,
                                ConfirmPassword = c.Password,
                                IsLoggedIn = c.IsLoggedIn,
                                Address = c.Address,
                                Language = c.Language,
                                AreaId = c.AreaId,
                                Lat = c.Lat,
                                Lng = c.Lng,
                                ModificationDate = c.ModificationDate,
                                Code = c.Code,
                                AddressDetails = c.AddressDetails,
                                FullName = c.FullName,
                                PageName = c.PageName,
                                Averageorders = c.Averageorders,
                                ProductType = c.ProductType,
                                VisaPayment = c.VisaPayment,
                                CreationDate = c.CreationDate,
                                Apartment = c.Apartment,
                                Floor = c.Floor,
                                Landmark = c.Landmark,
                                LocationUrl = c.LocationUrl,
                                WalletNumber = c.WalletNumber,
                                AccountName = c.AccountName,
                                AccountNumber = c.AccountNumber,
                                Bank = c.Bank,
                                IBANNumber = c.IbanNumber,
                                ImgUrl = c.ImgUrl,
                                InstaPayAccountName = c.InstaPayAccountName,
                                ZoneId = c.ZoneId,
                                IsDeleted = c.IsDeleted,
                                TelegramUserName = c.TelegramUserName,
                            });

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
                            .context.CommonUser.Where(c =>
                                !c.IsDeleted
                                && c.Email == model.Email
                                && c.Password == model.Password
                            )
                            .Select(c => new { user = c, role = c.Role })
                            .FirstOrDefault();

                        if (user != null)
                        {
                            //update user last login
                            user.user.LastLoginDate = DateTime.Now;
                            user.user.IsLoggedIn = true;
                            request.context.SaveChanges();

                            res.Message = HttpStatusCode.OK.ToString();
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;

                            res.UserDTOs = new List<UserDTO>
                            {
                                new UserDTO
                                {
                                    Id = user.user.Id,
                                    Email = user.user.Email,
                                    Name = user.user.Name,
                                    RoleID = user.user.RoleId,
                                    RoleName = user.role.Name,
                                    PhoneNumber = user.user.PhoneNumber,
                                    Password = user.user.Password,
                                    Language = user.user.Language,
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
                        var user = request.context.CommonUser.FirstOrDefault(c =>
                            !c.IsDeleted && c.Id == model.Id
                        );
                        if (user != null && user.Role.Name != SystemConstants.Role.SuperAdmin)
                        {
                            user.IsDeleted = true;
                            user.ModificationDate = DateTime.Now;
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
                        var user = request.context.CommonUser.Find(request.UserDTO.Id);

                        if (user != null)
                        {
                            var UserExist = request.context.CommonUser.Any(m =>
                                m.Email == request.UserDTO.Email
                                && m.Id != user.Id
                                && m.IsDeleted == false
                            );
                            if (!UserExist)
                            {
                                user.ModificationDate = DateTime.Now;
                                user.ModifiedBy = request.UserID;
                                if (request.UserDTO.RoleID != 0)
                                    user.RoleId = request.UserDTO.RoleID;

                                user.Name = request.UserDTO.Name;
                                user.Email = request.UserDTO.Email;
                                user.PhoneNumber = request.UserDTO.PhoneNumber;
                                user.AreaId = request.UserDTO.AreaId;
                                user.ZoneId = request.UserDTO.ZoneId;
                                user.Address = request.UserDTO.Address;
                                user.Name = request.UserDTO.Name;
                                user.FullName = request.UserDTO.FullName;
                                user.AddressDetails = request.UserDTO.AddressDetails;
                                user.ProductType = request.UserDTO.ProductType;
                                user.Averageorders = request.UserDTO.Averageorders;
                                user.PageName = request.UserDTO.PageName;
                                user.VisaPayment = request.UserDTO.VisaPayment;
                                user.VisaPayment = request.UserDTO.VisaPayment;
                                user.Landmark = request.UserDTO.Landmark;
                                user.LocationUrl = request.UserDTO.LocationUrl;
                                user.Floor = request.UserDTO.Floor;
                                user.Apartment = request.UserDTO.Apartment;
                                user.IsLoggedIn = request.UserDTO.IsLoggedIn;
                                user.Name = request.UserDTO.Name;
                                user.InstaPayAccountName = request.UserDTO.InstaPayAccountName;
                                user.IbanNumber = request.UserDTO.IBANNumber;
                                user.Bank = request.UserDTO.Bank;
                                user.WalletNumber = request.UserDTO.WalletNumber;
                                user.AccountName = request.UserDTO.AccountName;
                                user.AccountNumber = request.UserDTO.AccountNumber;
                                user.ImgUrl = request.UserDTO.ImgUrl;
                                user.TelegramUserName = request.UserDTO.TelegramUserName;
                                user.Gender = request.UserDTO.Gender;
                                user.Age = request.UserDTO.Age;
                                user.Language = request.UserDTO.Language;

                                request.context.SaveChanges();

                                res.UserDTO = request.UserDTO;
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

        public static UserResponse EditLanguage(UserRequest request)
        {
            var res = new UserResponse();
            RunBase(
                request,
                res,
                (UserRequest req) =>
                {
                    try
                    {
                        var user = request.context.CommonUser.Find(request.UserID);

                        if (user != null)
                        {
                            user.Language = request.UserDTO.Language;
                            request.context.SaveChanges();

                            res.UserDTO = request.UserDTO;
                            res.Message = "User updated successfully";
                            res.Success = true;
                            res.StatusCode = HttpStatusCode.OK;
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
                        if (request.UserDTO.NewPassword != request.UserDTO.ConfirmPassword)
                            return new UserResponse
                            {
                                Message = "Password didn't match",
                                Success = false
                            };

                        var user = request.context.CommonUser.Find(request.UserDTO.Id);
                        if (user != null)
                        {
                            user.ModificationDate = DateTime.Now;
                            user.ModifiedBy = request.UserID;
                            user.Password = request.UserDTO.NewPassword;

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

        public static async Task<UserResponse> AddUser(UserRequest request)
        {
            var res = new UserResponse();
            await RunBaseAsync<UserRequest, UserResponse>(
                request,
                res,
                async (UserRequest req) =>
                {
                    try
                    {
                        var emailExist = await request.context.CommonUser.AnyAsync(m =>
                            m.Email == request.UserDTO.Email && !m.IsDeleted
                        );
                        if (emailExist)
                        {
                            res.Message = "Email already exist";
                            res.Success = false;
                            res.StatusCode = HttpStatusCode.OK;
                            return res;
                        }
                        var phoneExist = await request.context.CommonUser.AnyAsync(m =>
                            m.PhoneNumber == request.UserDTO.PhoneNumber && !m.IsDeleted
                        );
                        if (phoneExist)
                        {
                            res.Message = "phone already exist";
                            res.Success = false;
                            res.StatusCode = HttpStatusCode.OK;
                            return res;
                        }

                        var User = new CommonUser();
                        User.CreationDate = DateTime.Now;
                        User.CreatedBy = request.UserID;
                        User.Code = Guid.NewGuid().ToString();
                        User.LastLoginDate = DateTime.Now;
                        User.IsDeleted = false;
                        User.IsLoggedIn = false;
                        User.Age = request.UserDTO.Age;
                        User.TelegramUserName = request.UserDTO.TelegramUserName;
                        User.CountryId = request.UserDTO.CountryId;
                        User.Password = request.UserDTO.Password;
                        User.HashedPassword = request.UserDTO.HashedPassword;
                        User.RoleId = request.UserDTO.RoleID;
                        User.Name = request.UserDTO.Name;
                        User.Email = request.UserDTO.Email;
                        User.PhoneNumber = request.UserDTO.PhoneNumber;
                        User.Name = request.UserDTO.Name;
                        User.NationalId = "EGY";
                        User.Address = request.UserDTO.Address;
                        User.FullName = request.UserDTO.FullName;
                        User.AddressDetails = request.UserDTO.AddressDetails;
                        User.Landmark = request.UserDTO.Landmark;
                        User.LocationUrl = request.UserDTO.LocationUrl;
                        User.Floor = request.UserDTO.Floor;
                        User.Apartment = request.UserDTO.Apartment;
                        User.AreaId = request.UserDTO.AreaId;
                        User.ZoneId = request.UserDTO.ZoneId;
                        User.Gender = request.UserDTO.Gender;
                        User.Bank = request.UserDTO.Bank;
                        User.IbanNumber = request.UserDTO.IBANNumber;
                        User.AccountName = request.UserDTO.AccountName;
                        User.AccountNumber = request.UserDTO.AccountNumber;
                        User.WalletNumber = request.UserDTO.WalletNumber;
                        User.InstaPayAccountName = request.UserDTO.InstaPayAccountName;
                        User.ImgUrl = request.UserDTO.ImgUrl;
                        User.Language = request.UserDTO.Language;

                        if (request.UserDTO.File == null)
                        {
                            var file = await request.avatarService.GetAvatarAsFormFileAsync(
                                request.UserDTO.Name
                            );

                            User.ImgUrl = BaseHelper.UploadImg(
                                file,
                                request.WebRootPath,
                                request.UserDTO.ImgUrl
                            );
                        }
                        else
                            User.ImgUrl = BaseHelper.UploadImg(
                                request.UserDTO.File,
                                request.WebRootPath,
                                request.UserDTO.ImgUrl
                            );
                        if (request.CountryService != null)
                        {
                            var countryCode = await request.CountryService.GetCountryInfoAsync(
                                request.UserIP
                            );

                            var countries = await request
                                .context.Countries.Where(x =>
                                    x.CountryCode == countryCode.country_code2
                                    || x.CountryCode == "EG"
                                )
                                .ToListAsync();
                            if (string.IsNullOrEmpty(countryCode.country_code2))
                                User.CountryId = countries
                                    .FirstOrDefault(x => x.CountryCode == "EG")
                                    .CountryId;
                            else
                                User.CountryId = countries
                                    .FirstOrDefault(x => x.CountryCode == countryCode.country_code2)
                                    .CountryId;
                        }
                        await request.context.CommonUser.AddAsync(User);
                        await request.context.SaveChangesAsync();

                        res.UserDTO = new UserDTO
                        {
                            Id = User.Id,
                            Name = User.Name,
                            Email = User.Email,
                            RoleID = User.RoleId,
                        };
                        res.Message = "New user added successfully";
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

        private static IQueryable<UserDTO> ApplyFilter(IQueryable<UserDTO> query, UserDTO filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(c =>
                    (c.Name.Contains(filter.Search))
                    || (c.PhoneNumber.Contains(filter.Search))
                    || (c.Email.Contains(filter.Search))
                    || (c.RoleName.Contains(filter.Search))
                );

            if (!string.IsNullOrEmpty(filter.RoleName))
                query = query.Where(c => c.RoleName == filter.RoleName);

            if (filter.Id > 0)
                query = query.Where(c => c.Id == filter.Id);

            if (filter.RoleID == (long)EnumRole.Gamer)
                query = query.Where(c => c.RoleID == filter.RoleID);

            if (filter.RoleID == (int)EnumRole.Employee)
                query = query.Where(u =>
                    u.RoleID != (int)EnumRole.Gamer
                    && u.RoleID != (int)EnumRole.Customer
                    && !u.IsDeleted
                );

            if (
                filter.RoleID > 0
                && filter.RoleID != (int)EnumRole.Gamer
                && filter.RoleID != (int)EnumRole.Employee
            )
                query = query.Where(c => c.RoleID == filter.RoleID);

            if (filter.StaffOnly)
                query = query.Where(c => c.RoleID != (int)EnumRole.Gamer);

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(c => c.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                query = query.Where(c =>
                    c.PhoneNumber.Trim()
                        .IndexOf(filter.PhoneNumber.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                );

            return query;
        }
    }
}
