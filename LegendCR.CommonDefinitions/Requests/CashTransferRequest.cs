using LegendCR.CommonDefinitions.DTO.CashDTOs;

namespace LegendCR.CommonDefinitions.Requests
{
    public class CashTransferRequest : BaseRequest
    {
        public CashTransferDTO CashTransferDTO { get; set; }
    }
}
