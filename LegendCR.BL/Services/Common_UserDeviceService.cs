//using System.Net;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.Helpers;

//namespace LegendCR.BL.Services
//{
//    public class Common_UserDeviceService : BaseService
//    {
//        public static Common_UserDeviceResponse ListCommon_UserDevice(
//            Common_UserDeviceRequest request
//        )
//        {
//            var res = new Common_UserDeviceResponse();
//            RunBase(
//                request,
//                res,
//                (Common_UserDeviceRequest req) =>
//                {
//                    try
//                    {
//                        var query = request.context.CommonUserDevice.Select(
//                            p => new Common_UserDeviceDTO
//                            {
//                                ID = p.Id,
//                                Common_UserID = p.CommonUserId,
//                                DeviceName = p.DeviceName,
//                                DeviceIMEI = p.DeviceImei,
//                                DeviceType = p.DeviceType,
//                                DeviceOSVersion = p.DeviceOsversion,
//                                DeviceToken = p.DeviceToken,
//                                DeviceEmail = p.DeviceEmail,
//                                EnableNotification = p.EnableNotification ?? false,
//                                AuthToken = p.AuthToken,
//                                AuthIP = p.AuthIp,
//                                AuthCreationDate = p.AuthCreationDate,
//                                AuthExpirationDate = p.AuthExpirationDate,
//                                IsLoggedIn = p.IsLoggedIn,
//                                DeviceMobileNumber = p.DeviceMobileNumber,
//                                LastActiveDate = p.LastActiveDate,
//                                Lang = p.Lang,
//                                CreatedBy = p.CreatedBy,
//                                ModifiedBy = p.ModifiedBy,
//                                LastUpdateDate = p.LastUpdateDate,
//                                CreationDate = p.CreationDate,
//                                IsDeleted = p.IsDeleted,
//                                IsGoogleSupport = p.IsGoogleSupport,
//                            }
//                        );

//                        if (request.Common_UserDeviceRecord != null)
//                            query = ApplyFilter(query, request.Common_UserDeviceRecord);

//                        res.TotalCount = query.Count();

//                        query = OrderByDynamic(
//                            query,
//                            request.OrderByColumn ?? "id",
//                            request.IsDesc
//                        );

//                        if (request.PageSize > 0)
//                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

//                        res.Common_UserDeviceRecords = query.ToList();
//                        res.Message = HttpStatusCode.OK.ToString();
//                        res.Success = true;
//                        res.StatusCode = HttpStatusCode.OK;
//                    }
//                    catch (Exception ex)
//                    {
//                        res.Message = ex.Message;
//                        res.Success = false;
//                        LogHelper.LogException(ex.Message, ex.StackTrace);
//                    }
//                    return res;
//                }
//            );
//            return res;
//        }

//        public static Common_UserDeviceResponse DeleteCommon_UserDevice(
//            Common_UserDeviceRequest request
//        )
//        {
//            var res = new Common_UserDeviceResponse();
//            RunBase(
//                request,
//                res,
//                (Common_UserDeviceRequest req) =>
//                {
//                    try
//                    {
//                        var model = request.Common_UserDeviceRecord;
//                        var Common_UserDevice = request.context.CommonUserDevice.FirstOrDefault(c =>
//                            c.Id == model.ID
//                        );
//                        if (Common_UserDevice != null)
//                        {
//                            //update Agency IsDeleted
//                            request.context.SaveChanges();

//                            res.Message = MessageKey.DeletedSuccessfully.ToString();
//                            res.Success = true;
//                            res.StatusCode = HttpStatusCode.OK;
//                        }
//                        else
//                        {
//                            res.Message = MessageKey.InvalidData.ToString();
//                            res.Success = false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        res.Message = ex.Message;
//                        res.Success = false;
//                        LogHelper.LogException(ex.Message, ex.StackTrace);
//                    }
//                    return res;
//                }
//            );
//            return res;
//        }

//        public static Common_UserDeviceResponse AddCommon_UserDevice(
//            Common_UserDeviceRequest request
//        )
//        {
//            var res = new Common_UserDeviceResponse();
//            RunBase(
//                request,
//                res,
//                (Common_UserDeviceRequest req) =>
//                {
//                    try
//                    {
//                        var Common_UserDevice = AddOrEditCommon_UserDevice(
//                            request.Common_UserDeviceRecord
//                        );
//                        request.context.CommonUserDevice.Add(Common_UserDevice);
//                        request.context.SaveChanges();

//                        res.Message = "AddedSuccessfully";
//                        res.Success = true;
//                        res.StatusCode = HttpStatusCode.OK;
//                    }
//                    catch (Exception ex)
//                    {
//                        res.Message = ex.Message;
//                        res.Success = false;
//                        LogHelper.LogException(ex.Message, ex.StackTrace);
//                    }
//                    return res;
//                }
//            );
//            return res;
//        }

//        public static Common_UserDeviceResponse EditCommon_UserDevice(
//            Common_UserDeviceRequest request
//        )
//        {
//            var res = new Common_UserDeviceResponse();
//            RunBase(
//                request,
//                res,
//                (Common_UserDeviceRequest req) =>
//                {
//                    try
//                    {
//                        var model = request.Common_UserDeviceRecord;
//                        var Common_UserDevice = request.context.CommonUserDevice.Find(model.ID);
//                        if (Common_UserDevice != null)
//                        {
//                            //update whole Agency
//                            Common_UserDevice = AddOrEditCommon_UserDevice(
//                                request.Common_UserDeviceRecord,
//                                Common_UserDevice
//                            );
//                            request.context.SaveChanges();

//                            res.Message = "UpdatedSuccessfully";
//                            res.Success = true;
//                            res.StatusCode = HttpStatusCode.OK;
//                        }
//                        else
//                        {
//                            res.Message = "Invalid";
//                            res.Success = false;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        res.Message = ex.Message;
//                        res.Success = false;
//                        LogHelper.LogException(ex.Message, ex.StackTrace);
//                    }
//                    return res;
//                }
//            );
//            return res;
//        }

//        private static CommonUserDevice AddOrEditCommon_UserDevice(
//            Common_UserDeviceDTO common_UserDeviceRecord,
//            CommonUserDevice common_UserDevice = null
//        )
//        {
//            if (common_UserDevice == null)
//            {
//                common_UserDevice = new CommonUserDevice();
//            }

//            common_UserDevice.CommonUserId = common_UserDeviceRecord.Common_UserID;
//            common_UserDevice.DeviceName = common_UserDeviceRecord.DeviceName;
//            common_UserDevice.DeviceImei = common_UserDeviceRecord.DeviceIMEI;
//            common_UserDevice.DeviceType = common_UserDeviceRecord.DeviceType;
//            common_UserDevice.DeviceOsversion = common_UserDeviceRecord.DeviceOSVersion;
//            common_UserDevice.DeviceToken = common_UserDeviceRecord.DeviceToken;
//            common_UserDevice.DeviceEmail = common_UserDeviceRecord.DeviceEmail;
//            common_UserDevice.EnableNotification = common_UserDeviceRecord.EnableNotification;
//            common_UserDevice.AuthToken = common_UserDeviceRecord.AuthToken;
//            common_UserDevice.AuthIp = common_UserDeviceRecord.AuthIP;
//            common_UserDevice.AuthCreationDate = common_UserDeviceRecord.AuthCreationDate;
//            common_UserDevice.AuthExpirationDate = common_UserDeviceRecord.AuthExpirationDate;
//            common_UserDevice.IsLoggedIn = common_UserDeviceRecord.IsLoggedIn;
//            common_UserDevice.DeviceMobileNumber = common_UserDeviceRecord.DeviceMobileNumber;
//            common_UserDevice.LastActiveDate = common_UserDeviceRecord.LastActiveDate.Value;
//            common_UserDevice.Lang = common_UserDeviceRecord.Lang;
//            common_UserDevice.CreatedBy = common_UserDeviceRecord.CreatedBy;
//            common_UserDevice.ModifiedBy = common_UserDeviceRecord.ModifiedBy;
//            common_UserDevice.LastUpdateDate = common_UserDeviceRecord.LastUpdateDate.Value;
//            common_UserDevice.CreationDate = common_UserDeviceRecord.CreationDate;
//            common_UserDevice.IsDeleted = common_UserDeviceRecord.IsDeleted;
//            common_UserDevice.IsGoogleSupport = common_UserDeviceRecord.IsGoogleSupport;
//            return common_UserDevice;
//        }

//        private static IQueryable<Common_UserDeviceDTO> ApplyFilter(
//            IQueryable<Common_UserDeviceDTO> query,
//            Common_UserDeviceDTO record
//        )
//        {
//            if (record.Common_UserID > 0)
//            {
//                query = query.Where(p => p.Common_UserID == record.Common_UserID);
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceName))
//            {
//                query = query.Where(p =>
//                    p.DeviceName != null && p.DeviceName.Contains(record.DeviceName)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceIMEI))
//            {
//                query = query.Where(p =>
//                    p.DeviceIMEI != null && p.DeviceIMEI.Contains(record.DeviceIMEI)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceType))
//            {
//                query = query.Where(p =>
//                    p.DeviceType != null && p.DeviceType.Contains(record.DeviceType)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceOSVersion))
//            {
//                query = query.Where(p =>
//                    p.DeviceOSVersion != null && p.DeviceOSVersion.Contains(record.DeviceOSVersion)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceToken))
//            {
//                query = query.Where(p =>
//                    p.DeviceToken != null && p.DeviceToken.Contains(record.DeviceToken)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.DeviceEmail))
//            {
//                query = query.Where(p =>
//                    p.DeviceEmail != null && p.DeviceEmail.Contains(record.DeviceEmail)
//                );
//            }

//            if (!string.IsNullOrWhiteSpace(record.AuthToken))
//            {
//                query = query.Where(p =>
//                    p.AuthToken != null && p.AuthToken.Contains(record.AuthToken)
//                );
//            }
//            if (!string.IsNullOrWhiteSpace(record.AuthIP))
//            {
//                query = query.Where(p => p.AuthIP != null && p.AuthIP.Contains(record.AuthIP));
//            }

//            if (!string.IsNullOrWhiteSpace(record.DeviceMobileNumber))
//            {
//                query = query.Where(p =>
//                    p.DeviceMobileNumber != null
//                    && p.DeviceMobileNumber.Contains(record.DeviceMobileNumber)
//                );
//            }
//            if (record.LastActiveDate.HasValue)
//            {
//                query = query.Where(p => p.LastActiveDate == record.LastActiveDate);
//            }
//            if (!string.IsNullOrWhiteSpace(record.Lang))
//            {
//                query = query.Where(p => p.Lang != null && p.Lang.Contains(record.Lang));
//            }

//            if (record.LastUpdateDate.HasValue)
//            {
//                query = query.Where(p => p.LastUpdateDate == record.LastUpdateDate);
//            }

//            if (record.IsGoogleSupport.HasValue)
//            {
//                query = query.Where(p => p.IsGoogleSupport == record.IsGoogleSupport);
//            }
//            return query;
//        }
//    }
//}
