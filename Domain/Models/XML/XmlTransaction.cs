using Domain.Enums;
using System;

namespace Domain.Models.XML
{
    public class XmlTransaction
    {
        public string TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public CurrencyEnum CurrencyCode { get; set; }
        public XmlTransactionStatusEnum Status { get; set; }
    }
}
