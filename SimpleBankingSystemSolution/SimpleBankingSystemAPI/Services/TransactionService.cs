using AutoMapper;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Repositories;
using System.Security.Principal;

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

        public async Task DepositAsync(Guid userId, Guid accountId, DepositRequest request)
        {
            var user = await _userRepository.GetById(userId);
            var account = await _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            if (account.isActive == false)
            {
                throw new AccountNotActivedException("Account is not activated");
            }

            if (request.Amount <= 0)
            {
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
                Timestamp = System.DateTime.UtcNow,
                Description = "Deposit",
                IsRecurring = false
            };
            await _transactionRepository.Add(transaction);
            await _accountRepository.Update(account);
            await _emailService.SendEmailAsync(user.Email!, "Deposit Successful [Simple Bank] ", $"Amount Deposited : {request.Amount} \n New Balance : {account.Balance}\n If this was not you? contact CustomerService");
        }

        public async Task WithdrawAsync(Guid userId, Guid accountId, DepositRequest request)
        {
            var user = await _userRepository.GetById(userId);

            var account = await _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            if (account.isActive == false)
            {
                throw new AccountNotActivedException("Account is not activated");
            }

            if (request.Amount <= 0)
            {
                throw new InvalidTransactionException("Invalid amount");
            }

            if (account.Balance < request.Amount)
            {
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
                Timestamp = System.DateTime.UtcNow,
                Description = "Withdraw",
                IsRecurring = false
            };
            await _transactionRepository.Add(transaction);
            await _accountRepository.Update(account);
            await _emailService.SendEmailAsync(user.Email!, "Withdraw Successful [Simple Bank] ", $"Amount Withdrawn : {request.Amount} \n New Balance : {account.Balance}\n If this was not you? contact CustomerService");

        }
        public async Task TransferAsync(Guid userId, Guid accountId, Guid receiverId, BankTransferRequest request)
        {
            if(request.TransactionType != "IMPS" && request.TransactionType != "NEFT" && request.TransactionType != "RTFS")
            {
                throw new InvalidTransactionException("Invalid transaction type");
            }

            var user = await _userRepository.GetById(userId);
            var account = await _accountRepository.GetById(accountId);
            var receiverAccount = await _accountRepository.GetById(receiverId);
            var verificationCheck = await _transactionVerificationRepository.GetVerificationByAccountIdAsync(accountId);

            if (verificationCheck != null)
            {
                throw new TransactionAlreadyInProgressException("Trasaction Already Exists!");
            }

            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            if (account.isActive == false)
            {
                throw new AccountNotActivedException("Your Account is not activated");
            }
            if (receiverAccount == null)
            {
                throw new AccountNotFoundException("Receiver account not found");
            }
            if (receiverAccount.isActive == false)
            {
                throw new AccountNotActivedException("Receiver Account is not activated");
            }
            if (request.Amount <= 0) {
                throw new InvalidAmountException("Invalid amount");
            }
            if (account.Balance < request.Amount)
            {
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
                Timestamp = System.DateTime.UtcNow,
                
            };
            await _emailService.SendEmailAsync(user.Email!, $"Verify your {request.TransactionType} transaction ", $"Payment to {receiverId} \nYour verification code is: {verificationCode}");
            await _transactionVerificationRepository.Add(verification);
        }
        public async Task <TransactionDto>TransferVerificationAsync(Guid userId, Guid accountId, string verificationCode)
        {
            
            var user = await _userRepository.GetById(userId);
            var account = await _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            var verification = await _transactionVerificationRepository.GetVerificationByAccountIdAsync(accountId);

            if (verification == null)
            {
                throw new TransactionVerificationNotFoundException("Verification not found");
            }

            if (verification.VerificationCode != verificationCode)
            {
                throw new InvalidVerificationCodeException("Invalid verification code");
            }

            if (verification.Timestamp.AddMinutes(5) < System.DateTime.UtcNow)
            {
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
            await _emailService.SendEmailAsync(senderAccount.User!.Email!, "IMPS Transaction Successful [Simple Bank] ", $"Payment to {receiverAccount.User!.FirstName} {receiverAccount.User!.LastName} \n\n Transaction Amount : {verification.Amount} \n New Balance : {senderAccount.Balance}");
            await _emailService.SendEmailAsync(receiverAccount.User!.Email!, "IMPS Transaction Received [Simple Bank] ", $"Payment from {senderAccount.User!.FirstName} {senderAccount.User!.LastName} \n\n Transaction Amount : {verification.Amount} \n New Balance : {receiverAccount.Balance}");
            await _transactionRepository.Add(_mapper.Map<Transaction>(verification));
            await _transactionVerificationRepository.Delete(verification.Id);
            }
            else
            {
                var PendingTransaction = new PendingAccountTransaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = verification.AccountId,
                    ReceiverId = verification.ReceiverId,
                    Amount = verification.Amount,
                    TransactionType = verification.TransactionType,
                    IsApproved = false,
                    IsRejected = false,
                    Timestamp = System.DateTime.UtcNow,
                };
                await _pendingAccountTransactionRepository.Add(PendingTransaction);
            }

            return _mapper.Map<TransactionDto>(verification);
        }
        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            var transactions = await _transactionRepository.GetTransactionsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        //To be Added to USER
        public async Task<IEnumerable<TransactionRequestDto>> GetTransactionByAccountAsync(Guid userId, Guid accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            var transactions = await _pendingAccountTransactionRepository.GetAllTransactionByAccountId(accountId);
            return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionRequestDto>> GetTransactionRequestAsync()
        {
            var transactions = await _pendingAccountTransactionRepository.GetAll();
            return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionRequestDto>> GetPendingTransactionRequestAsync()
        {
            var transactions = await _pendingAccountTransactionRepository.GetAllPendingTransactions();
            return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionRequestDto>> GetRejectedTransactionRequestAsync()
        {
            var transactions = await _pendingAccountTransactionRepository.GetAllRejectedTransactions();
            return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionRequestDto>> GetApprovedTransactionRequestAsync()
        {
            var transactions = await _pendingAccountTransactionRepository.GetAllAcceptedTransactions();
            return _mapper.Map<IEnumerable<TransactionRequestDto>>(transactions);
        }


        public async Task ApproveTransaction(Guid requestId)
        {
            var request = await _pendingAccountTransactionRepository.GetById(requestId);
            if (request == null)
            {
                throw new TransactionNotFoundException("Transaction not found");
            }
            var senderAccount = await _accountRepository.GetById(request.AccountId);
            var receiverAccount = await _accountRepository.GetById(request.ReceiverId);
            senderAccount.Balance -= request.Amount;
            receiverAccount.Balance += request.Amount;

            request.IsApproved = true;

            var senderUser = await _userRepository.GetById(senderAccount.UserId);
            var receiverUser = await _userRepository.GetById(receiverAccount.UserId);
            await _emailService.SendEmailAsync(senderUser.Email!, $" {request.TransactionType} Transaction Successful [Simple Bank] ", $"Payment to {receiverUser.FirstName} {receiverUser.LastName} \n\n Transaction Amount : {request.Amount} \n New Balance : {senderAccount.Balance}");
            await _emailService.SendEmailAsync(senderUser.Email!, $" {request.TransactionType} Transaction Received [Simple Bank] ", $"Payment from {senderUser.FirstName} {senderUser.LastName} \n\n Transaction Amount : {request.Amount} \n New Balance : {receiverAccount.Balance}");
            await _pendingAccountTransactionRepository.Update(request);
            await _accountRepository.Update(senderAccount);
            await _accountRepository.Update(receiverAccount);
        }
        public async Task RejectTransaction (Guid requestId)
        {
            var request = await _pendingAccountTransactionRepository.GetById(requestId);
            if (request == null)
            {
                throw new TransactionNotFoundException("Transaction not found");
            }
            request.IsRejected = true;

            var senderAccount = await _accountRepository.GetById(request.AccountId);
            var receiverAccount = await _accountRepository.GetById(request.ReceiverId);
            var senderUser = await _userRepository.GetById(senderAccount.UserId);
            var receiverUser = await _userRepository.GetById(receiverAccount.UserId);
            await _emailService.SendEmailAsync(senderUser.Email!, $" {request.TransactionType} Transaction Failed [Simple Bank] ", $"Payment to {receiverUser.FirstName} {receiverUser.LastName} \n\n Transaction Amount : {request.Amount} \n");
            await _pendingAccountTransactionRepository.Update(request);
        }
    }
}
