using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Records;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DicomApp.BL.Services
{
    public class TransactionService : BaseService
    {
        public static TransactionResponse GetTransactions(TransactionRequest request)
        {
            var res = new TransactionResponse();
            RunBase(
                request,
                res,
                (TransactionRequest req) =>
                {
                    try
                    {
                        var query = request
                            .context.Transaction.Where(z => !z.IsDeleted)
                            .Select(p => new TransactionDTO
                            {
                                AdvertisementId = p.AdvertisementId,
                                Amount = p.Amount,
                                BuyerId = p.BuyerId,
                                CreatedAt = p.CreatedAt,
                                CreatedBy = p.CreatedBy,
                                IsDeleted = p.IsDeleted,
                                IsSuccess = p.IsSuccess,
                                Attachment = p.Attachment,
                                PaymentId = p.PaymentId,
                                LastModifiedAt = p.LastModifiedAt,
                                LastModifiedBy = p.LastModifiedBy,
                                RefId = p.RefId,
                                TransactionId = p.TransactionId,
                                ServiceId = p.ServiceId,
                                TransactionType = p.TransactionType,
                                TransactionSource = p.TransactionSource
                            });

                        if (request.TransactionDTO != null)
                            query = ApplyFilter(query, request.TransactionDTO);

                        query = OrderByDynamic(
                            query,
                            request.OrderByColumn ?? nameof(Transaction.CreatedAt),
                            request.IsDesc
                        );

                        if (request.PageSize > 0)
                            query = ApplyPaging(query, request.PageSize, request.PageIndex);

                        res.TransactionDTOs = query.ToList();
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

        public static TransactionResponse SuccessPay(TransactionRequest request)
        {
            var res = new TransactionResponse();
            RunBase(
                request,
                res,
                (TransactionRequest req) =>
                {
                    try
                    {
                        var transaction = request.context.Transaction.FirstOrDefault(x =>
                            x.PaymentId == request.TransactionDTO.PaymentId
                        );
                        var advertisement = request.context.Advertisement.FirstOrDefault(x =>
                            x.AdvertisementId == request.TransactionDTO.AdvertisementId
                        );
                        if (transaction != null)
                            transaction.IsSuccess = true;

                        if (advertisement != null && transaction != null)
                            advertisement.BuyerId = transaction.BuyerId;

                        request.context.SaveChanges();
                        res.Message = "Pay Successfully";
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

        public static TransactionResponse AddTransaction(TransactionRequest request)
        {
            var res = new TransactionResponse();
            RunBase(
                request,
                res,
                (TransactionRequest req) =>
                {
                    try
                    {
                        var Transaction = new Transaction()
                        {
                            AdvertisementId = req.TransactionDTO.AdvertisementId,
                            Amount = req.TransactionDTO.Amount,
                            PaymentId = req.TransactionDTO.PaymentId,
                            IsSuccess = false,
                            ServiceId = req.TransactionDTO.ServiceId,
                            TransactionType = req.TransactionDTO.TransactionType,
                            TransactionSource = req.TransactionDTO.TransactionSource,
                            Attachment = req.TransactionDTO.Attachment,
                            BuyerId = req.UserID,
                            CreatedBy = req.UserID,
                            CreatedAt = DateTime.Now,
                        };
                        request.context.Transaction.Add(Transaction);
                        request.context.SaveChanges();
                        Transaction.RefId = BaseHelper.GenerateRefId(
                            EnumRefIdType.Transaction,
                            Transaction.TransactionId
                        );
                        request.context.SaveChanges();
                        res.Message = "Added Successfully";
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

        private static IQueryable<TransactionDTO> ApplyFilter(
            IQueryable<TransactionDTO> query,
            TransactionDTO record
        )
        {
            if (record.AdvertisementId > 0)
                query = query.Where(p => p.AdvertisementId == record.AdvertisementId);

            return query;
        }
    }
}
