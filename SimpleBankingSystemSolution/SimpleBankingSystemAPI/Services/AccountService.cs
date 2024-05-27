using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IPendingAccountClosingRepository _pendingAccountClosingRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(ILogger<AccountService> logger,IPendingAccountClosingRepository pendingAccountClosingRepository,IUserRepository userRepository,IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _pendingAccountClosingRepository = pendingAccountClosingRepository;
            _logger = logger;
        }

        public async Task<AccountDto> OpenAccountAsync(Guid userId, OpenAccountRequest request)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                _logger.LogError("User not found");
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
                UpdatedDate = System.DateTime.UtcNow
            };

            await _accountRepository.Add(account);
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> GetAccountAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }
            return _mapper.Map<AccountDto>(account);
        }

        public async Task RequestCloseAccountAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }

            account.UpdatedDate = System.DateTime.UtcNow;
            account.AccountType = "Closed";
            _accountRepository.Update(account);

        }

        public async Task<IEnumerable<AccountDto>> GetAccountsAsync(Guid userId)
        {
            var accounts =  await _accountRepository.GetAccountsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAll();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            await _accountRepository.Delete(accountId);
        }

        public async Task<AccountDto> UpdateAccountAsync(Guid userId, Guid accountId, UpdateAccountRequest request)
        {
            var account = await _accountRepository.GetById(accountId);
        
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }
            CreatePasswordHash(request.TransactionPassword!, out byte[] passwordHash, out byte[] passwordSalt);

            account.TransactionPasswordHash = passwordHash;
            account.TransactionPasswordKey = passwordSalt;
            _accountRepository.Update(account);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task CloseAccountAsync(Guid userId, Guid accountId,AccountClosingDto request)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null || account.UserId != userId)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if(account.Balance>0)
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
        }


        public async Task<IEnumerable<AccountClosingDto>> GetPendingAccountClosingRequests(Guid userId)
        {
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
        public async Task AcceptAccountCloseRequest (Guid userId , Guid requestId)
        {
            var user = await _userRepository.GetById(userId);
            var closeAccountRequest = await _pendingAccountClosingRepository.GetById(requestId);
            if (user == null)
            {
                throw new UserNotFoundException("Authorization Failure");
            }
            if(user.Role != "Admin")
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
            
        }

        public async Task RejectAccountCloseRequest(Guid userId, Guid requestId)
        {
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
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
