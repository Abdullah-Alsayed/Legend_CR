using System;
using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.DTO.AdvertisementDTOs
{
    public class AdsDTO
    {
        public int AdvertisementId { get; set; }
        public string RefId { get; set; }
        public int GameId { get; set; }
        public int StatusId { get; set; }
        public int GamerId { get; set; }
        public int? BuyerId { get; set; }
        public int? AdvertisementRequestId { get; set; }

        public string Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int Level { get; set; }
        public string Rank { get; set; }
        public int Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public GameDTO Game { get; set; }
        public StatusDTO Status { get; set; }
        public UserDTO Gamer { get; set; }
        public UserDTO Buyer { get; set; }

        public List<IFormFile> Files { get; set; }
        public CashTransferDTO CashTransfer { get; set; }
        public List<string> AdvertisementPhotos { get; set; }
        public List<AccountTransactionDTO> AccountTransaction { get; set; }
        public List<FollowUpDTO> FollowUp { get; set; }
        public bool SELECTED { get; set; }

        // Filter

        public bool? Publish { get; set; }
        public string Search { get; set; }
        public string VendorName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<int> AdvertisementIds { get; set; }
        public List<string> FilesUrls { get; set; }
        public int StatusType { get; set; }
        public int LessPrice { get; set; }
        public int GreeterPrice { get; set; }
        public int LessLevel { get; set; }
        public int GreeterLevel { get; set; }
        public List<int> StatusTypes { get; set; }
    }
}
