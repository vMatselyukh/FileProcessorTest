using AutoMapper;
using Domain.Enums;
using Domain.Interfaces.Managers;
using Domain.Interfaces.Services;
using Domain.Models.Api;
using System;
using System.Collections.Generic;

namespace Bll.Managers
{
    public class TransactionManager : ITransactionManager
    {
        private ITransactionService _transactionService;
        private IMapper _automapper;
        public TransactionManager(ITransactionService transactionService, IMapper automapper)
        {
            _transactionService = transactionService;
            _automapper = automapper;
        }

        public List<ApiTransaction> GetAll()
        {
            var transactions = _transactionService.GetAll();
            return _automapper.Map<List<ApiTransaction>>(transactions);
        }

        public List<ApiTransaction> GetByCurrency(CurrencyEnum currency)
        {
            var transactions = _transactionService.GetByCurrency(currency);
            return _automapper.Map<List<ApiTransaction>>(transactions);
        }

        public List<ApiTransaction> GetByDateRange(DateTime beginDate, DateTime endDate)
        {
            var transactions = _transactionService.GetByDateRange(beginDate, endDate);
            return _automapper.Map<List<ApiTransaction>>(transactions);
        }

        public List<ApiTransaction> GetByStatus(TransactionStatusEnum status)
        {
            var transactions = _transactionService.GetByStatus(status);
            return _automapper.Map<List<ApiTransaction>>(transactions);
        }
    }
}
