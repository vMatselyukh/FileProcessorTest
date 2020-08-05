using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using EfContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class TransactionService : ITransactionService
    {
        private IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InsertListAsync(List<Transaction> transactions)
        {
            await _unitOfWork.TransactionRepository.InsertListAsync(transactions);
            await _unitOfWork.SaveWithTransactionAsync();
        }
    }
}
