using Domain.Enums;
using EfContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task InsertListAsync(List<Transaction> transactions);
        List<Transaction> GetAll();
        List<Transaction> GetByCurrency(CurrencyEnum currency);
        List<Transaction> GetByStatus(TransactionStatusEnum status);
        List<Transaction> GetByDateRange(DateTime beginDate, DateTime endDate);
    }
}
