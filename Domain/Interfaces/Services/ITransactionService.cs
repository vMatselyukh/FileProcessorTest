using EfContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ITransactionService
    {
        Task InsertListAsync(List<Transaction> transactions);
    }
}
