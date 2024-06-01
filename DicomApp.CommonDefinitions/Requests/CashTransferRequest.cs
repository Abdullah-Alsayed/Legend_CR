using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class CashTransferRequest : BaseRequest
    {
        public CashTransferDTO CashTransferDTO { get; set; }
    }
}
