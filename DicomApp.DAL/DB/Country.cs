using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DicomApp.DAL.DB
{
    public class Country
    {
        public int CountryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string FlagUrl { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<CommonUser> Users { get; set; }
    }
}
