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
            throw new NotImplementedException();
        }

        public List<Transaction> GetByDateRange(DateTime beginDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public List<Transaction> GetByStatus(TransactionStatusEnum status)
        {
            throw new NotImplementedException();
        }

        public async Task InsertList(List<Transaction> transactions)
        {
            await _dbContext.Transactions.AddRangeAsync(transactions);
        }
    }
}
