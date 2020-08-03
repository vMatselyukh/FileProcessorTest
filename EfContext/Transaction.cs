using System;

namespace EfContext
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
    }
}
