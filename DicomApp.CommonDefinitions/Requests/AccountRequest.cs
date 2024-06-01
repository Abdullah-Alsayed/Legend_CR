using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class AccountRequest : BaseRequest
    {
        public AccountDTO AccountDTO { get; set; }
        public bool HasTransactionsDTOs { get; set; }
    }

}
