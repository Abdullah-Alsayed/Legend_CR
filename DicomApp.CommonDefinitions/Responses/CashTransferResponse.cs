using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class CashTransferResponse : BaseResponse
    {
        public CashTransferDTO CashTransferDTO { get; set; }
        public InvoiceDTO InvoiceDTO { get; set; }
    }
}
