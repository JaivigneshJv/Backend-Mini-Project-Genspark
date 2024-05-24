using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Interfaces.Repositories;

namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface IAccountRepository : IRepository<Guid,Account>
    {
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(Guid userId);
    }
}
