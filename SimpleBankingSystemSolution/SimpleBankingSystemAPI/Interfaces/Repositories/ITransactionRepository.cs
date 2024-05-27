using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Guid, Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId);
    }
}
