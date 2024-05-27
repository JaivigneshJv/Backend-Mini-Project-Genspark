using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface IPendingAccountTransactionRepository : IRepository<Guid, PendingAccountTransaction>
    {
       Task <IEnumerable<PendingAccountTransaction>> GetAllTransactionByAccountId(Guid accountId);
       Task<IEnumerable<PendingAccountTransaction>> GetAllPendingTransactions();
       Task<IEnumerable<PendingAccountTransaction>> GetAllAcceptedTransactions();
       Task<IEnumerable<PendingAccountTransaction>> GetAllRejectedTransactions();
    }
}
