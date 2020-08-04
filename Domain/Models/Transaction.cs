using Domain.Enums;
using System;

namespace Domain.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public TransactionStatusEnum Status { get; set; }
    }
}
