using System.Collections.Generic;

namespace Domain.Models
{
    public class FileParseResult
    {
        public bool IsSucceed { get; set; }
        public string ErrorMessage { get; set; }
        public List<Transaction> TransactionList { get; set; }
    }
}
