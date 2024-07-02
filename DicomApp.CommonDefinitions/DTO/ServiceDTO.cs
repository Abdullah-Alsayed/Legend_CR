using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO
{
    public class ServiceDTO
    {
        public int GamerId { get; set; }
        public int GameId { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Price { get; set; }
        public int Level { get; set; }
        public string Rank { get; set; }
        public int GamerServiceId { get; set; }
        public int StatusId { get; set; }
        public GameServiceType GameServiceType { get; set; }
        public string RefId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public StatusDTO Status { get; set; }
        public List<FollowUpDTO> FollowUp { get; set; }
        public GameDTO Game { get; set; }
        public UserDTO Gamer { get; set; }
        public List<int> GamerServiceIds { get; set; }
        public string Search { get; set; }
        public int LessLevel { get; set; }
        public int GreeterLevel { get; set; }
        public int LessPrice { get; set; }
        public int GreeterPrice { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int StatusType { get; set; }
    }
}
