using Domain.Enums;
using EfContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAll();
        Task InsertList(List<Transaction> transactions);
        List<Transaction> GetByStatus(TransactionStatusEnum status);
        List<Transaction> GetByDateRange(DateTime beginDate, DateTime endDate);
        List<Transaction> GetByCurrency(CurrencyEnum currency);
    }
}
