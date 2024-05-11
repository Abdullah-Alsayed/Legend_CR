using LegendCR.CommonDefinitions.DTO.CashDTOs;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class AccountTransactionResponse : BaseResponse
    {
        public AccountTransactionDTO AccountTransactionDTO { get; set; }
        public List<AccountTransactionDTO> AccountTransactionDTOs { get; set; }
    }
}

