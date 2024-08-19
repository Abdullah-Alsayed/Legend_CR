using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DicomApp.DAL.DB
{
    public partial class GamerService
    {
        public GamerService()
        {
            FollowUp = new HashSet<FollowUp>();
        }

        public int GamerServiceId { get; set; }
        public string RefId { get; set; }
        public int GameId { get; set; }
        public int StatusId { get; set; }

        [ForeignKey(nameof(Gamer))]
        public int GamerId { get; set; }

        public string Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CurrentLevel { get; set; }
        public string Level { get; set; }
        public int Price { get; set; }
        public GameServiceType GameServiceType { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Game Game { get; set; }
        public virtual Status Status { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual CommonUser Gamer { get; set; }
        public virtual ICollection<FollowUp> FollowUp { get; set; }
    }
}
