using System;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository TransactionRepository { get; }
        Task Save();
    }
}
