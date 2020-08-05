using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using EfContext;
using System;
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

        public List<Transaction> GetByCurrency(CurrencyEnum currency)
        {
            return _unitOfWork.TransactionRepository.GetByCurrency(currency);
        }

        public List<Transaction> GetByStatus(TransactionStatusEnum status)
        {
            return _unitOfWork.TransactionRepository.GetByStatus(status);
        }

        public List<Transaction> GetByDateRange(DateTime beginDate, DateTime endDate)
        {
            return _unitOfWork.TransactionRepository.GetByDateRange(beginDate, endDate);
        }

        public List<Transaction> GetAll()
        {
            return _unitOfWork.TransactionRepository.GetAll();
        }
    }
}
