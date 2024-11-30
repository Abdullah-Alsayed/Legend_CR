using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO.CashDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class TransactionResponse : BaseResponse
    {
        public TransactionDTO TransactionDTO { get; set; }
        public List<TransactionDTO> TransactionDTOs { get; set; }
    }
}
