using System;

namespace EF
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}
