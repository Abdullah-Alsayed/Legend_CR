using DicomApp.CommonDefinitions.DTO.CashDTOs;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class AccountResponse : BaseResponse
    {
        public AccountDTO AccountDTO { get; set; }
        public List<AccountDTO> AccountDTOs { get; set; }
        public List<TreasuryDTO> TreasuryDTOs { get; set; }
        public TreasuryDTO TreasuryDTO { get; set; }
    }
}

