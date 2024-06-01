using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class AccountTransactionRequest : BaseRequest
    {
        public AccountTransactionDTO AccountTransactionDTO { get; set; }
    }
}

