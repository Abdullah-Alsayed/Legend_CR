using LegendCR.CommonDefinitions.DTO.CashDTOs;

namespace LegendCR.CommonDefinitions.Responses
{
    public class CashTransferResponse : BaseResponse
    {
        public CashTransferDTO CashTransferDTO { get; set; }
        public InvoiceDTO InvoiceDTO { get; set; }
    }
}
