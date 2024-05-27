using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface ITransactionVerificationRepository : IRepository<Guid, TransactionVerification>
    {
        Task<TransactionVerification> GetVerificationByAccountIdAsync(Guid accountId);
        
    }
}
