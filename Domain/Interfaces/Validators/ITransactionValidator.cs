using Domain.Models;
using Domain.Models.Validation;
using System.Collections.Generic;

namespace Domain.Interfaces.Validators
{
    public interface ITransactionValidator
    {
        ValidationResult ValidateTransaction(Transaction transaction);
        ValidationResult ValidateTransactions(List<Transaction> transaction);
    }
}
