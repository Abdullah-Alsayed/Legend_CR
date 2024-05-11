//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.DTO.CashDTOs;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.CommonDefinitions.Responses;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using System;
//using System.Linq;
//using System.Net;

//namespace LegendCR.BL.Services
//{
//    public class AccountTransactionService : BaseService
//    {
//        public static bool AddTransaction(ApplicationDBContext context, AccountTransactionDTO dto)
//        {
//            try
//            {
//                var transaction = new AccountTransaction
//                {
//                    TypeId = dto.TypeId,
//                    SenderId = dto.SenderId,
//                    ReceiverId = dto.ReceiverId,
//                    VendorId = dto.VendorId,
//                    CashTransferId = dto.CashTransferId,

//                    StatusId = dto.StatusId,
//                    ShipmentId = dto.ShipmentId,
//                    PickupRequestId = dto.PickupRequestId,

//                    PackingFees = dto.PackingFees,
//                    WeightFees = dto.WeightFees,
//                    SizeFees = dto.SizeFees,
//                    PartialDeliveryFees = dto.PartialDeliveryFees,
//                    CancelFees = dto.CancelFees,
//                    PickupFees = dto.PickupFees,
//                    ShippingFees = dto.ShippingFees,
//                    ShippingFeesPaid = dto.ShippingFeesPaid,
//                    VendorCash = dto.VendorCash,
//                    Total = dto.Total,
//                    RefundCash = dto.RefundCash,
//                    RefundFees = dto.RefundFees,

//                    CreatedAt = DateTime.Now,
//                    LastModifiedAt = DateTime.Now,
//                    CreatedBy = dto.CreatedBy,
//                    LastModifiedBy = dto.CreatedBy,
//                    IsDeleted = false,
//                };

//                context.AccountTransaction.Add(transaction);
//                context.SaveChanges();

//                transaction.RefId = BaseHelper.GenerateRefId(EnumRefIdType.Account_Transaction, transaction.AccountTransactionId, transaction.TypeId);

//                //if (string.IsNullOrEmpty(transaction.Otp))
//                //{
//                // START : Update Account BALANCE           ***** START DANGER AREA *****
//                var account = context.Account.FirstOrDefault(s => s.AccountId == dto.ReceiverId);
//                account.LastModifiedAt = DateTime.Now;
//                account.LastModifiedBy = dto.CreatedBy;

//                if (transaction.TypeId == (byte)EnumTransactionType.Deposit)
//                    account.Balance += transaction.Total;
//                else if (transaction.TypeId == (byte)EnumTransactionType.Withdraw)
//                    account.Balance -= transaction.Total;

//                context.SaveChanges();
//                // END : Update Account BALANCE             ***** END DANGER AREA *****
//                //}

//                return true;
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogException(ex.Message, ex.StackTrace);
//                return false;
//            }
//        }

//        public static AccountTransactionResponse GetTransaction(AccountTransactionRequest request)
//        {
//            var response = new AccountTransactionResponse();
//            RunBase(request, response, (AccountTransactionRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.AccountTransaction.Where(s => s.AccountTransactionId == request.AccountTransactionDTO.AccountTransactionId && !s.IsDeleted)
//                    .Select(s => new AccountTransactionDTO
//                    {
//                        AccountTransactionId = s.AccountTransactionId,
//                        RefId = s.RefId,
//                        TypeId = s.TypeId,
//                        SenderId = s.SenderId,
//                        ReceiverId = s.ReceiverId,
//                        VendorId = s.VendorId,
//                        StatusId = s.StatusId,
//                        StatusName = s.StatusId.HasValue ? s.Status.Name : null,
//                        ShipmentId = s.ShipmentId,
//                        PickupRequestId = s.PickupRequestId,
//                        PackingFees = s.PackingFees,
//                        WeightFees = s.WeightFees,
//                        SizeFees = s.SizeFees,
//                        PartialDeliveryFees = s.PartialDeliveryFees,
//                        CancelFees = s.CancelFees,
//                        PickupFees = s.PickupFees,
//                        ShippingFees = s.ShippingFees,
//                        ShippingFeesPaid = s.ShippingFeesPaid,
//                        VendorCash = s.VendorCash,
//                        Total = s.Total,
//                        CreatedAt = s.CreatedAt,
//                        LastModifiedAt = s.LastModifiedAt,
//                        CreatedBy = s.CreatedBy,
//                        LastModifiedBy = s.LastModifiedBy,

//                        CashTransferId = s.CashTransferId,
//                        CashTransferRefId = s.CashTransferId.HasValue ? s.CashTransfer.RefId : null,

//                        RefundCash = s.RefundCash,
//                        RefundFees = s.RefundFees,
//                    });

//                    response.AccountTransactionDTO = query.FirstOrDefault();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        public static AccountTransactionResponse GetAllTransactions(AccountTransactionRequest request)
//        {
//            var response = new AccountTransactionResponse();
//            RunBase(request, response, (AccountTransactionRequest req) =>
//            {
//                try
//                {
//                    var query = request.context.AccountTransaction.Where(s => !s.IsDeleted)
//                    .Select(s => new AccountTransactionDTO
//                    {
//                        AccountTransactionId = s.AccountTransactionId,
//                        RefId = s.RefId,
//                        TypeId = s.TypeId,
//                        SenderId = s.SenderId,
//                        ReceiverId = s.ReceiverId,
//                        VendorId = s.VendorId,
//                        StatusId = s.StatusId,
//                        StatusName = s.StatusId.HasValue ? s.Status.Name : null,
//                        ShipmentId = s.ShipmentId,
//                        PickupRequestId = s.PickupRequestId,
//                        PackingFees = s.PackingFees,
//                        WeightFees = s.WeightFees,
//                        SizeFees = s.SizeFees,
//                        PartialDeliveryFees = s.PartialDeliveryFees,
//                        CancelFees = s.CancelFees,
//                        PickupFees = s.PickupFees,
//                        ShippingFees = s.ShippingFees,
//                        ShippingFeesPaid = s.ShippingFeesPaid,
//                        VendorCash = s.VendorCash,
//                        Total = s.Total,
//                        CreatedAt = s.CreatedAt,
//                        LastModifiedAt = s.LastModifiedAt,
//                        CreatedBy = s.CreatedBy,
//                        LastModifiedBy = s.LastModifiedBy,

//                        CashTransferId = s.CashTransferId,
//                        CashTransferRefId = s.CashTransferId.HasValue ? s.CashTransfer.RefId : null,

//                        RefundCash = s.RefundCash,
//                        RefundFees = s.RefundFees,

//                    });

//                    if (request.applyFilter)
//                        query = ApplyFilter(query, request.AccountTransactionDTO, request.RoleID, request.UserID);

//                    response.AccountTransactionDTOs = query.ToList();

//                    response.Message = HttpStatusCode.OK.ToString();
//                    response.Success = true;
//                    response.StatusCode = HttpStatusCode.OK;
//                }
//                catch (Exception ex)
//                {
//                    response.Message = ex.Message;
//                    response.Success = false;
//                    LogHelper.LogException(ex.Message, ex.StackTrace);
//                }
//                return response;
//            });
//            return response;
//        }

//        private static IQueryable<AccountTransactionDTO> ApplyFilter(IQueryable<AccountTransactionDTO> query, AccountTransactionDTO filter, int RoleId, int UserId)
//        {
//            if (filter.SenderId > 0)
//                query = query.Where(p => p.SenderId == filter.SenderId);

//            if (filter.ReceiverId > 0)
//                query = query.Where(p => p.ReceiverId == filter.ReceiverId);

//            if (filter.VendorId > 0)
//                query = query.Where(p => p.VendorId == filter.VendorId);

//            if (filter.ShipmentId > 0)
//                query = query.Where(p => p.ShipmentId == filter.ShipmentId);


//            if (filter.PickupRequestId > 0)
//                query = query.Where(p => p.PickupRequestId == filter.PickupRequestId);

//            return query;
//        }
//    }
//}