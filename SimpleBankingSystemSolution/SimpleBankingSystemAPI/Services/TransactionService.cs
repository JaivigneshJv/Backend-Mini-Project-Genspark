using AutoMapper;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Repositories;
using System.Security.Principal;
using WatchDog;

namespace SimpleBankingSystemAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailVerificationRepository _emailRepository;
        private readonly IEmailSender _emailService;
        private readonly ITransactionVerificationRepository _transactionVerificationRepository;
        private readonly IPendingAccountTransactionRepository _pendingAccountTransactionRepository;

        public TransactionService(
            ITransactionVerificationRepository transactionVerificationRepository,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IEmailVerificationRepository emailRepository,
            IEmailSender emailService,
            IMapper mapper,
            ITransactionRepository transactionRepository,
            IPendingAccountTransactionRepository pendingAccountTransactionRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _emailRepository = emailRepository;
            _emailService = emailService;
            _pendingAccountTransactionRepository = pendingAccountTransactionRepository;
            _transactionVerificationRepository = transactionVerificationRepository;
        }

        /// <summary>
        /// Performs a deposit operation for a specified user and account.
        /// </summary>
        /// <param name="userId">The ID of the user performing the deposit.</param>
        /// <param name="accountId">The ID of the account to deposit into.</param>
        /// <param name="request">The deposit request containing the amount to deposit.</param>
        public async Task DepositAsync(Guid userId, Guid accountId, DepositRequest request)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to deposit {request.Amount} to account ID: {accountId}");
                var user = await _userRepository.GetById(userId);
                var account = await _accountRepository.GetById(accountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }
                if (!account.isActive)
                {
                    WatchLogger.LogWarning($"Account is not activated: {accountId}");
                    throw new AccountNotActivedException("Account is not activated");
                }
                if (request.Amount <= 0)
                {
                    WatchLogger.LogWarning($"Invalid deposit amount: {request.Amount}");
                    throw new InvalidTransactionException("Invalid amount");
                }

                account.Balance += request.Amount;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    ReceiverId = accountId,
                    Amount = request.Amount,
                    TransactionType = "Deposit",
                    Timestamp = DateTime.UtcNow,
                    Description = "Deposit",
                    IsRecurring = false
                };
                await _transactionRepository.Add(transaction);
                await _accountRepository.Update(account);
                await _emailService.SendEmailAsync(user.Email!, "Deposit Successful [Simple Bank]", $"Amount Deposited: {request.Amount} \nNew Balance: {account.Balance}\nIf this was not you, contact CustomerService");
                WatchLogger.Log($"Deposit successful for account ID: {accountId}, Amount: {request.Amount}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during deposit operation: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Performs a withdrawal operation for a specified user and account.
        /// </summary>
        /// <param name="userId">The ID of the user performing the withdrawal.</param>
        /// <param name="accountId">The ID of the account to withdraw from.</param>
        /// <param name="request">The withdrawal request containing the amount to withdraw.</param>
        public async Task WithdrawAsync(Guid userId, Guid accountId, DepositRequest request)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to withdraw {request.Amount} from account ID: {accountId}");
                var user = await _userRepository.GetById(userId);
                var account = await _accountRepository.GetById(accountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }
                if (!account.isActive)
                {
                    WatchLogger.LogWarning($"Account is not activated: {accountId}");
                    throw new AccountNotActivedException("Account is not activated");
                }
                if (request.Amount <= 0)
                {
                    WatchLogger.LogWarning($"Invalid withdrawal amount: {request.Amount}");
                    throw new InvalidTransactionException("Invalid amount");
                }
                if (account.Balance < request.Amount)
                {
                    WatchLogger.LogWarning($"Insufficient funds for withdrawal: {accountId}");
                    throw new InsufficientFundsException("Insufficient funds");
                }

                account.Balance -= request.Amount;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    ReceiverId = accountId,
                    Amount = request.Amount,
                    TransactionType = "Withdraw",
                    Timestamp = DateTime.UtcNow,
                    Description = "Withdraw",
                    IsRecurring = false
                };
                await _transactionRepository.Add(transaction);
                await _accountRepository.Update(account);
                await _emailService.SendEmailAsync(user.Email!, "Withdraw Successful [Simple Bank]", $"Amount Withdrawn: {request.Amount} \nNew Balance: {account.Balance}\nIf this was not you, contact CustomerService");
                WatchLogger.Log($"Withdrawal successful for account ID: {accountId}, Amount: {request.Amount}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during withdrawal operation: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Performs a transfer operation from one account to another.
        /// </summary>
        /// <param name="userId">The ID of the user performing the transfer.</param>
        /// <param name="accountId">The ID of the account to transfer from.</param>
        /// <param name="receiverId">The ID of the account to transfer to.</param>
        /// <param name="request">The transfer request containing the amount and transaction type.</param>
        public async Task TransferAsync(Guid userId, Guid accountId, Guid receiverId, BankTransferRequest request)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to transfer {request.Amount} from account ID: {accountId} to receiver ID: {receiverId}");
                if (request.TransactionType != "IMPS" && request.TransactionType != "NEFT" && request.TransactionType != "RTGS")
                {
                    WatchLogger.LogWarning($"Invalid transaction type: {request.TransactionType}");
                    throw new InvalidTransactionException("Invalid transaction type");
                }

                var user = await _userRepository.GetById(userId);
                var account = await _accountRepository.GetById(accountId);
                var receiverAccount = await _accountRepository.GetById(receiverId);
                var verificationCheck = await _transactionVerificationRepository.GetVerificationByAccountIdAsync(accountId);

                if (verificationCheck != null)
                {
                    WatchLogger.LogWarning($"Transaction already in progress for account ID: {accountId}");
                    throw new TransactionAlreadyInProgressException("Transaction already exists!");
                }

                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }
                if (!account.isActive)
                {
                    WatchLogger.LogWarning($"Account is not activated: {accountId}");
                    throw new AccountNotActivedException("Your account is not activated");
                }
                if (receiverAccount == null)
                {
                    WatchLogger.LogWarning($"Receiver account not found for ID: {receiverId}");
                    throw new AccountNotFoundException("Receiver account not found");
                }
                if (!receiverAccount.isActive)
                {
                    WatchLogger.LogWarning($"Receiver account is not activated: {receiverId}");
                    throw new AccountNotActivedException("Receiver account is not activated");
                }
                if (request.Amount <= 0)
                {
                    WatchLogger.LogWarning($"Invalid transfer amount: {request.Amount}");
                    throw new InvalidAmountException("Invalid amount");
                }
                if (account.Balance < request.Amount)
                {
                    WatchLogger.LogWarning($"Insufficient funds for transfer: {accountId}");
                    throw new InsufficientFundsException("Insufficient funds");
                }

                var verificationCode = new Random().Next(100000, 999999).ToString();
                var verification = new TransactionVerification
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    ReceiverId = receiverId,
                    Amount = request.Amount,
                    TransactionType = request.TransactionType,
                    VerificationCode = verificationCode,
                    Timestamp = DateTime.UtcNow,
                };
                await _emailService.SendEmailAsync(user.Email!, $"Verify your {request.TransactionType} transaction", $"Payment to {receiverId} \nYour verification code is: {verificationCode}");
                await _transactionVerificationRepository.Add(verification);
                WatchLogger.Log($"Transfer verification initiated for account ID: {accountId}, Amount: {request.Amount}, Transaction Type: {request.TransactionType}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during transfer operation: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Verifies a transfer operation for a specified user and account using a verification code.
        /// </summary>
        /// <param name="userId">The ID of the user performing the verification.</param>
        /// <param name="accountId">The ID of the account to verify the transfer for.</param>
        /// <param name="verificationCode">The verification code to validate the transfer.</param>
        /// <returns>The verified transaction details.</returns>
        public async Task<TransactionDto> TransferVerificationAsync(Guid userId, Guid accountId, string verificationCode)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to verify transfer for account ID: {accountId} with verification code: {verificationCode}");
                var user = await _userRepository.GetById(userId);
                var account = await _accountRepository.GetById(accountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }

                var verification = await _transactionVerificationRepository.GetVerificationByAccountIdAsync(accountId);
                if (verification == null)
                {
                    WatchLogger.LogWarning($"Verification not found for account ID: {accountId}");
                    throw new TransactionVerificationNotFoundException("Verification not found");
                }
                if (verification.VerificationCode != verificationCode)
                {
                    WatchLogger.LogWarning($"Invalid verification code: {verificationCode} for account ID: {accountId}");
                    throw new InvalidVerificationCodeException("Invalid verification code");
                }
                if (verification.Timestamp.AddMinutes(5) < DateTime.UtcNow)
                {
                    WatchLogger.LogWarning($"Verification code expired for account ID: {accountId}");
                    throw new VerificationCodeExpiredException("Verification code expired");
                }

                if (verification.TransactionType == "IMPS")
                {
                    var senderAccount = await _accountRepository.GetById(verification.AccountId);
                    var receiverAccount = await _accountRepository.GetById(verification.ReceiverId);

                    senderAccount.Balance -= verification.Amount;
                    receiverAccount.Balance += verification.Amount;

                    await _accountRepository.Update(account);
                    await _accountRepository.Update(receiverAccount);
                    await _emailService.SendEmailAsync(senderAccount.User!.Email!, "IMPS Transaction Successful [Simple Bank]", $"Payment to {receiverAccount.User!.FirstName} {receiverAccount.User!.LastName} \n\n Transaction Amount: {verification.Amount} \nNew Balance: {senderAccount.Balance}");
                    await _emailService.SendEmailAsync(receiverAccount.User!.Email!, "IMPS Transaction Received [Simple Bank]", $"Payment from {senderAccount.User!.FirstName} {senderAccount.User!.LastName} \n\n Transaction Amount: {verification.Amount} \nNew Balance: {receiverAccount.Balance}");
                    await _transactionRepository.Add(_mapper.Map<Transaction>(verification));
                    await _transactionVerificationRepository.Delete(verification.Id);
                }
                else
                {
                    var pendingTransaction = new PendingAccountTransaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = verification.AccountId,
                        ReceiverId = verification.ReceiverId,
                        Amount = verification.Amount,
                        TransactionType = verification.TransactionType,
                        IsApproved = false,
                        IsRejected = false,
                        Timestamp = DateTime.UtcNow,
                    };
                    await _pendingAccountTransactionRepository.Add(pendingTransaction);
                }

                WatchLogger.Log($"Transfer verification successful for account ID: {accountId}, Amount: {verification.Amount}, Transaction Type: {verification.TransactionType}");
                return _mapper.Map<TransactionDto>(verification);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error during transfer verification: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all transactions for a specified user and account.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve transactions for.</param>
        /// <param name="accountId">The ID of the account to retrieve transactions for.</param>
        /// <returns>A collection of transaction details.</returns>
        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(Guid userId, Guid accountId)
        {
            try
            {
                WatchLogger.Log($"Fetching transactions for account ID: {accountId}");
                var account = await _accountRepository.GetById(accountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }
                var transactions = await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
                WatchLogger.Log($"Transactions fetched successfully for account ID: {accountId}");
                return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching transactions for account ID: {accountId}: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all transaction requests for a specified user and account.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve transaction requests for.</param>
        /// <param name="accountId">The ID of the account to retrieve transaction requests for.</param>
        /// <returns>A collection of transaction request details.</returns>
        public async Task<IEnumerable<TransactionRequestDto>> GetTransactionByAccountAsync(Guid userId, Guid accountId)
        {
            try
            {
                WatchLogger.Log($"Fetching transaction requests for account ID: {accountId}");
                var account = await _accountRepository.GetById(accountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {accountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {accountId}");
                    throw new AccessViolationException("Access Denied");
                }
                var transactions = await _pendingAccountTransactionRepository.GetAllTransactionByAccountId(accountId);
                WatchLogger.Log($"Transaction requests fetched successfully for account ID: {accountId}");
                return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching transaction requests for account ID: {accountId}: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all transaction requests.
        /// </summary>
        /// <returns>A collection of transaction request details.</returns>
        public async Task<IEnumerable<TransactionRequestDto>> GetTransactionRequestAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all transaction requests");
                var transactions = await _pendingAccountTransactionRepository.GetAll();
                WatchLogger.Log("Transaction requests fetched successfully");
                return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching transaction requests: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all pending transaction requests.
        /// </summary>
        /// <returns>A collection of pending transaction request details.</returns>
        public async Task<IEnumerable<TransactionRequestDto>> GetPendingTransactionRequestAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all pending transaction requests");
                var transactions = await _pendingAccountTransactionRepository.GetAllPendingTransactions();
                WatchLogger.Log("Pending transaction requests fetched successfully");
                return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching pending transaction requests: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all rejected transaction requests.
        /// </summary>
        /// <returns>A collection of rejected transaction request details.</returns>
        public async Task<IEnumerable<TransactionRequestDto>> GetRejectedTransactionRequestAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all rejected transaction requests");
                var transactions = await _pendingAccountTransactionRepository.GetAllRejectedTransactions();
                WatchLogger.Log("Rejected transaction requests fetched successfully");
                return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching rejected transaction requests: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Retrieves all approved transaction requests.
        /// </summary>
        /// <returns>A collection of approved transaction request details.</returns>
        public async Task<IEnumerable<TransactionRequestDto>> GetApprovedTransactionRequestAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all approved transaction requests");
                var transactions = await _pendingAccountTransactionRepository.GetAllAcceptedTransactions();
                WatchLogger.Log("Approved transaction requests fetched successfully");
                return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching approved transaction requests: {ex}");
                throw;
            }
        }
        
        /// <summary>
        /// Approves a transaction request.
        /// </summary>
        /// <param name="requestId">The ID of the transaction request to approve.</param>
        public async Task ApproveTransaction(Guid requestId)
        {
            try
            {
                WatchLogger.Log($"Approving transaction request ID: {requestId}");
                var request = await _pendingAccountTransactionRepository.GetById(requestId);
                if (request == null)
                {
                    WatchLogger.LogWarning($"Transaction request not found for ID: {requestId}");
                    throw new TransactionNotFoundException("Transaction not found");
                }
                var senderAccount = await _accountRepository.GetById(request.AccountId);
                var receiverAccount = await _accountRepository.GetById(request.ReceiverId);
                senderAccount.Balance -= request.Amount;
                receiverAccount.Balance += request.Amount;

                request.IsApproved = true;

                var senderUser = await _userRepository.GetById(senderAccount.UserId);
                var receiverUser = await _userRepository.GetById(receiverAccount.UserId);
                await _emailService.SendEmailAsync(senderUser.Email!, $" {request.TransactionType} Transaction Successful [Simple Bank]", $"Payment to {receiverUser.FirstName} {receiverUser.LastName} \n\n Transaction Amount: {request.Amount} \nNew Balance: {senderAccount.Balance}");
                await _emailService.SendEmailAsync(receiverUser.Email!, $" {request.TransactionType} Transaction Received [Simple Bank]", $"Payment from {senderUser.FirstName} {senderUser.LastName} \n\n Transaction Amount: {request.Amount} \nNew Balance: {receiverAccount.Balance}");
                await _pendingAccountTransactionRepository.Update(request);
                await _accountRepository.Update(senderAccount);
                await _accountRepository.Update(receiverAccount);
                WatchLogger.Log($"Transaction request approved for ID: {requestId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error approving transaction request ID: {requestId}: {ex}");
                throw;
            }
        }
        

        /// <summary>
        /// Rejects a transaction request.
        /// </summary>
        /// <param name="requestId">The ID of the transaction request to reject.</param>
        public async Task RejectTransaction(Guid requestId)
        {
            try
            {
                WatchLogger.Log($"Rejecting transaction request ID: {requestId}");
                var request = await _pendingAccountTransactionRepository.GetById(requestId);
                if (request == null)
                {
                    WatchLogger.LogWarning($"Transaction request not found for ID: {requestId}");
                    throw new TransactionNotFoundException("Transaction not found");
                }
                request.IsRejected = true;

                var senderAccount = await _accountRepository.GetById(request.AccountId);
                var receiverAccount = await _accountRepository.GetById(request.ReceiverId);
                var senderUser = await _userRepository.GetById(senderAccount.UserId);
                var receiverUser = await _userRepository.GetById(receiverAccount.UserId);
                await _emailService.SendEmailAsync(senderUser.Email!, $" {request.TransactionType} Transaction Failed [Simple Bank]", $"Payment to {receiverUser.FirstName} {receiverUser.LastName} \n\n Transaction Amount: {request.Amount}");
                await _pendingAccountTransactionRepository.Update(request);
                WatchLogger.Log($"Transaction request rejected for ID: {requestId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error rejecting transaction request ID: {requestId}: {ex}");
                throw;
            }
        }
        
    }
}
