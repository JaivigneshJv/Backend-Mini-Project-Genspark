using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Account> OpenAccountAsync(Guid userId, OpenAccountRequest request);
        Task<Account> GetAccountAsync(Guid userId, Guid accountId);
        Task RequestCloseAccountAsync(Guid userId, Guid accountId);
        Task<IEnumerable<Account>> GetAccountsAsync(Guid userId);

        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task DeleteAccountAsync(Guid accountId);
    }
}
