
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class FollowUpType
    {
        public FollowUpType()
        {
            FollowUp = new HashSet<FollowUp>();
        }

        public int FollowUpTypeId { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<FollowUp> FollowUp { get; set; }
    }
}
