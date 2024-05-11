using LegendCR.CommonDefinitions.DTO.CashDTOs;

namespace LegendCR.CommonDefinitions.Requests
{
    public class AccountRequest : BaseRequest
    {
        public AccountDTO AccountDTO { get; set; }
        public bool HasTransactionsDTOs { get; set; }
    }

}
