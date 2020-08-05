using Domain.Enums;
using Domain.Interfaces.Repositories;
using EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private TransactionContext _dbContext;

        public TransactionRepository(TransactionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Transaction> GetAll()
        {
            return _dbContext.Transactions.ToList();
        }

        public List<Transaction> GetByCurrency(CurrencyEnum currency)
        {
            return _dbContext.Transactions.Where(t => t.Currency == currency.ToString()).ToList();
        }

        public List<Transaction> GetByDateRange(DateTime beginDate, DateTime endDate)
        {
            return _dbContext.Transactions.Where(t => t.Date >= beginDate && t.Date <= endDate).ToList();
        }

        public List<Transaction> GetByStatus(TransactionStatusEnum status)
        {
            return _dbContext.Transactions.Where(t => t.Status == (int)status).ToList();
        }

        public async Task InsertListAsync(List<Transaction> transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(transactions);
        }
    }
}
