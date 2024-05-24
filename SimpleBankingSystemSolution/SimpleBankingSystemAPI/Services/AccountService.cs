using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using System.Text;

namespace SimpleBankingSystemAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<Account> OpenAccountAsync(Guid userId, OpenAccountRequest request)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            CreatePasswordHash(request.TransactionPassword, out byte[] passwordHash, out byte[] passwordSalt);

            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountType = request.AccountType,
                Balance = 0,
                TransactionPasswordHash = passwordHash,
                TransactionPasswordKey = passwordSalt
            };

            return await _accountRepository.Add(account);
        }

        public async Task<Account> GetAccountAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }

            return account;
        }

        public async Task RequestCloseAccountAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }

            // Logic for request close account (e.g., mark for closure)
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(Guid userId)
        {
            return await _accountRepository.GetAccountsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAll();
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            await _accountRepository.Delete(accountId);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
