using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DicomApp.DAL.DB;
using System.Linq;
using DicomApp.Helpers;

namespace DicomApp.CommonDefinitions.DTO
{
    public class UserhubconnectionDTO
    {
       



        public int ID { get; set; }


        public string ConnectionID { get; set; }


        public bool IsOnline { get; set; }


        public int? CreatedBy { get; set; }
		
		[IgnoreDataMember]		
        public DateTime CreationDate { get; set; }
		
        public string CreationDateStr
        {
            get
            {
                return DateTimeHelper.ToDate(CreationDate);
            }
            set
            {
               
            }
        }

		
		[IgnoreDataMember]		
        public DateTime? ModificationDate { get; set; }
		
        public string ModificationDateStr
        {
            get
            {
                return DateTimeHelper.ToDate(ModificationDate ?? DateTime.Now);
            }
            set
            {
               
            }
        }



        public int? ModifiedBy { get; set; }


        public bool IsDeleted { get; set; }

    }

}
