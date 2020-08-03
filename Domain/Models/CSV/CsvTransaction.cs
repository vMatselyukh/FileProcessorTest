using System;

namespace Domain.Models.CSV
{
    public class CsvTransaction
    {
        public string TransactionId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public DateTime Date { get; set; }
        public CsvTransactionStatusEnum Status { get; set; }
    }
}
