using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface IAccountRepository : IRepository<Guid, Account>
    {
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(Guid userId);
    }
}
