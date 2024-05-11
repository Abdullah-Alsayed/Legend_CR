using LegendCR.CommonDefinitions.DTO.CashDTOs;

namespace LegendCR.CommonDefinitions.Requests
{
    public class AccountTransactionRequest : BaseRequest
    {
        public AccountTransactionDTO AccountTransactionDTO { get; set; }
    }
}

