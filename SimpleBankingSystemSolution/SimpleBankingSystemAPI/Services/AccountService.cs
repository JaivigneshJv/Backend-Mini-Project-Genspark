using AutoMapper;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using System.Text;
using WatchDog;

namespace SimpleBankingSystemAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPendingAccountClosingRepository _pendingAccountClosingRepository;

        public AccountService(IPendingAccountClosingRepository pendingAccountClosingRepository, IUserRepository userRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _pendingAccountClosingRepository = pendingAccountClosingRepository;
        }

        /// <summary>
        /// Opens a new account for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="request">The request object containing the account details.</param>
        /// <returns>The created account DTO.</returns>
        public async Task<AccountDto> OpenAccountAsync(Guid userId, OpenAccountRequest request)
        {
            try
            {
                WatchLogger.Log($"Attempting to open account for user {userId}");
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                CreatePasswordHash(request.TransactionPassword!, out byte[] passwordHash, out byte[] passwordSalt);

                var account = new Account
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    AccountType = request.AccountType,
                    Balance = request.InitialDeposit,
                    TransactionPasswordHash = passwordHash,
                    TransactionPasswordKey = passwordSalt,
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow,
                    isActive = true
                };

                await _accountRepository.Add(account);
                WatchLogger.Log($"Account created successfully: {account.Id}");

                return _mapper.Map<AccountDto>(account);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Retrieves an account for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The account DTO.</returns>
        public async Task<AccountDto> GetAccountAsync(Guid userId, Guid accountId)
        {
            try
            {
                WatchLogger.Log($"Fetching account {accountId} for user {userId}");

                var account = await _accountRepository.GetById(accountId);
                if (account == null || account.UserId != userId)
                {
                    throw new AccountNotFoundException("Account not found");
                }

                return _mapper.Map<AccountDto>(account);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Requests to close an account for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="accountId">The ID of the account.</param>
        public async Task RequestCloseAccountAsync(Guid userId, Guid accountId)
        {
            try
            {
                WatchLogger.Log($"User {userId} requesting to close account {accountId}");

                var account = await _accountRepository.GetById(accountId);
                if (account == null || account.UserId != userId)
                {
                    throw new AccountNotFoundException("Account not found");
                }

                account.UpdatedDate = System.DateTime.UtcNow;
                account.AccountType = "Close Request";
                _accountRepository.Update(account);

                WatchLogger.Log($"Account {accountId} marked for closing");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Retrieves all accounts for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of account DTOs.</returns>
        public async Task<IEnumerable<AccountDto>> GetAccountsAsync(Guid userId)
        {
            try
            {
                WatchLogger.Log($"Fetching accounts for user {userId}");

                var accounts = await _accountRepository.GetAccountsByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<AccountDto>>(accounts);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Retrieves all accounts.
        /// </summary>
        /// <returns>A collection of account DTOs.</returns>
        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all accounts");

                var accounts = await _accountRepository.GetAll();
                return _mapper.Map<IEnumerable<AccountDto>>(accounts);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deletes an account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        public async Task DeleteAccountAsync(Guid accountId)
        {
            try
            {
                WatchLogger.Log($"Deleting account {accountId}");

                await _accountRepository.Delete(accountId);
                WatchLogger.Log($"Account {accountId} deleted");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Updates an account for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <param name="request">The request object containing the updated account details.</param>
        /// <returns>The updated account DTO.</returns>
        public async Task<AccountDto> UpdateAccountAsync(Guid userId, Guid accountId, UpdateAccountRequest request)
        {
            try
            {
                WatchLogger.Log($"Updating account {accountId} for user {userId}");

                var account = await _accountRepository.GetById(accountId);

                if (account == null || account.UserId != userId)
                {
                    throw new AccountNotFoundException("Account not found");
                }

                CreatePasswordHash(request.TransactionPassword!, out byte[] passwordHash, out byte[] passwordSalt);

                account.TransactionPasswordHash = passwordHash;
                account.TransactionPasswordKey = passwordSalt;
                _accountRepository.Update(account);

                WatchLogger.Log($"Account {accountId} updated");

                return _mapper.Map<AccountDto>(account);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Closes an account for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <param name="request">The request object containing the account closing details.</param>
        public async Task CloseAccountAsync(Guid userId, Guid accountId, AccountClosingDto request)
        {
            try
            {
                WatchLogger.Log($"User {userId} requesting to close account {accountId}");

                var account = await _accountRepository.GetById(accountId);
                if (account == null || account.UserId != userId)
                {
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.Balance > 0)
                {
                    throw new AccountHasBalanceException("Account has balance");
                }

                var pendingAccountClosing = new PendingAccountClosing
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    AccountType = request.AccountType,
                    RequestDate = System.DateTime.UtcNow,
                    Description = request.Description,
                    IsApproved = false,
                    IsRejected = false
                };

                await _pendingAccountClosingRepository.Add(pendingAccountClosing);
                WatchLogger.Log($"Pending account closing request created: {pendingAccountClosing.Id}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Retrieves all pending account closing requests for an admin user.
        /// </summary>
        /// <param name="userId">The ID of the admin user.</param>
        /// <returns>A collection of account closing DTOs.</returns>
        public async Task<IEnumerable<AccountClosingDto>> GetPendingAccountClosingRequests(Guid userId)
        {
            try
            {
                WatchLogger.Log($"Fetching pending account closing requests for admin {userId}");

                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }
                if (user.Role != "Admin")
                {
                    throw new AuthorizationException("Authorization Failure");
                }

                var pendingAccountClosingRequests = await _pendingAccountClosingRepository.GetAll();
                return _mapper.Map<IEnumerable<AccountClosingDto>>(pendingAccountClosingRequests);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Accepts an account close request by an admin user.
        /// </summary>
        /// <param name="userId">The ID of the admin user.</param>
        /// <param name="requestId">The ID of the account close request.</param>
        public async Task AcceptAccountCloseRequest(Guid userId, Guid requestId)
        {
            try
            {
                WatchLogger.Log($"Admin {userId} accepting account close request {requestId}");

                var user = await _userRepository.GetById(userId);
                var closeAccountRequest = await _pendingAccountClosingRepository.GetById(requestId);
                if (user == null)
                {
                    throw new UserNotFoundException("Authorization Failure");
                }
                if (user.Role != "Admin")
                {
                    throw new AuthorizationException("Authorization Failure");
                }

                var account = await _accountRepository.GetById(closeAccountRequest.AccountId);
                if (account == null)
                {
                    throw new AccountNotFoundException("Account not found");
                }
                if (closeAccountRequest == null)
                {
                    throw new PendingAccountClosingNotFoundException("Request not found");
                }

                closeAccountRequest.IsApproved = true;
                closeAccountRequest.IsRejected = false;
                _pendingAccountClosingRepository.Update(closeAccountRequest);

                account.AccountType = "Closed";
                account.UpdatedDate = System.DateTime.UtcNow;
                _accountRepository.Update(account);

                WatchLogger.Log($"Account close request {requestId} accepted and account {account.Id} closed");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Rejects an account close request by an admin user.
        /// </summary>
        /// <param name="userId">The ID of the admin user.</param>
        /// <param name="requestId">The ID of the account close request.</param>
        public async Task RejectAccountCloseRequest(Guid userId, Guid requestId)
        {
            try
            {
                WatchLogger.Log($"Admin {userId} rejecting account close request {requestId}");

                var user = await _userRepository.GetById(userId);
                var closeAccountRequest = await _pendingAccountClosingRepository.GetById(requestId);
                var account = await _accountRepository.GetById(closeAccountRequest.AccountId);
                if (user == null)
                {
                    throw new UserNotFoundException("Authorization Failure");
                }
                if (user.Role != "Admin")
                {
                    throw new AuthorizationException("Authorization Failure");
                }

                if (account == null)
                {
                    throw new AccountNotFoundException("Account not found");
                }
                if (closeAccountRequest == null)
                {
                    throw new PendingAccountClosingNotFoundException("Request not found");
                }

                closeAccountRequest.IsApproved = false;
                closeAccountRequest.IsRejected = true;
                _pendingAccountClosingRepository.Update(closeAccountRequest);

                WatchLogger.Log($"Account close request {requestId} rejected");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError(ex.ToString());
                throw;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
