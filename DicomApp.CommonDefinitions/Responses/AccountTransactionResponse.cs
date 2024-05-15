using DicomApp.CommonDefinitions.DTO.CashDTOs;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class AccountTransactionResponse : BaseResponse
    {
        public AccountTransactionDTO AccountTransactionDTO { get; set; }
        public List<AccountTransactionDTO> AccountTransactionDTOs { get; set; }
    }
}

