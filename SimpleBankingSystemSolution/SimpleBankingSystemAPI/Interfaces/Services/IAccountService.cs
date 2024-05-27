using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountDto> OpenAccountAsync(Guid userId, OpenAccountRequest request);
        Task<AccountDto> GetAccountAsync(Guid userId, Guid accountId);
        Task RequestCloseAccountAsync(Guid userId, Guid accountId);
        Task<IEnumerable<AccountDto>> GetAccountsAsync(Guid userId);
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task DeleteAccountAsync(Guid accountId);
        Task<AccountDto> UpdateAccountAsync(Guid userId, Guid accountId, UpdateAccountRequest request);
        Task CloseAccountAsync(Guid userId, Guid accountId,AccountClosingDto request);

        Task<IEnumerable<AccountClosingDto>> GetPendingAccountClosingRequests(Guid userId);
        Task AcceptAccountCloseRequest(Guid userId, Guid requestId);
        Task RejectAccountCloseRequest(Guid userId,  Guid requestId);
    }
}
