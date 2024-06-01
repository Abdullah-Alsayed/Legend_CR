
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Userhubconnection
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public bool IsOnline { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
