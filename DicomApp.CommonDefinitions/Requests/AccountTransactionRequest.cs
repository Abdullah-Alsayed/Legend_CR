using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class TransactionRequest : BaseRequest
    {
        public TransactionDTO TransactionDTO { get; set; }
    }
}
