using Domain.Interfaces.Repositories;
using EfContext;
using System;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionContext _transactionContext;
        private ITransactionRepository _transactionRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;

        public UnitOfWork(TransactionContext transactionContext, ITransactionRepository transactionRepository)
        {
            _transactionContext = transactionContext;
            _transactionRepository = transactionRepository;
        }

        public void Dispose()
        {
            _transactionContext.Dispose();
        }

        public async Task Save()
        {
            await _transactionContext.SaveChangesAsync();
        }
    }
}
