using Domain.Interfaces.Repositories;
using EfContext;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionContext _transactionContext;
        private ILogger<UnitOfWork> _logger;

        public ITransactionRepository TransactionRepository { get; }

        public UnitOfWork(TransactionContext transactionContext, ITransactionRepository transactionRepository,
            ILogger<UnitOfWork> logger)
        {
            _transactionContext = transactionContext;
            _logger = logger;

            TransactionRepository = transactionRepository;
        }

        public void Dispose()
        {
            _transactionContext.Dispose();
        }

        public async Task SaveAsync()
        {
            await _transactionContext.SaveChangesAsync();
        }

        public async Task SaveWithTransactionAsync()
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _transactionContext.SaveChangesAsync();
                    scope.Complete();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error during running transaction.");
                    throw e;
                }
            }
            
        }
    }
}
