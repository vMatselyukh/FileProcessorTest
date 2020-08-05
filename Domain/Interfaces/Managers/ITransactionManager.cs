using Domain.Enums;
using Domain.Models.Api;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces.Managers
{
    public interface ITransactionManager
    {
        List<ApiTransaction> GetAll();
        List<ApiTransaction> GetByStatus(TransactionStatusEnum status);
        List<ApiTransaction> GetByDateRange(DateTime beginDate, DateTime endDate);
        List<ApiTransaction> GetByCurrency(CurrencyEnum currency);
    }
}
