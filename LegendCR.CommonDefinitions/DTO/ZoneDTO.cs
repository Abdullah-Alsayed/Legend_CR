using LegendCR.CommonDefinitions.Records;

namespace LegendCR.CommonDefinitions.DTO
{
    public class ZoneDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public int? BranchId { get; set; }
        public virtual ZoneTaxDTO ZoneTax { get; set; }

        public virtual ICollection<CityDTO> City { get; set; }

        // For Search Only 
        public string Search { get; set; }
        public double Tax { get; set; }

        public List<int> AreaList { get; set; }
    }
}