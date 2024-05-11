namespace LegendCR.CommonDefinitions.DTO.ShipmentDTOs
{
    public class ShipProblemDTO
    {
        public int ShipmentProblemId { get; set; }
        public int ShipmentId { get; set; }
        public int ProblemTypeId { get; set; }
        public string ProblemTypeName { get; set; }     
        public bool IsSolved { get; set; }
        public string Solution { get; set; }
        public string Note { get; set; }
        public bool IsCourierProblem { get; set; }

        public bool IsReportToVendor { get; set; }
        public bool IsDeleted { get; set; }
    }
}