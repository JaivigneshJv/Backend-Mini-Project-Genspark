using AutoMapper;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using SimpleBankingSystemAPI.Repositories;

namespace SimpleBankingSystemAPI.Services
{
    public class LoanService : ILoanServices
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoanRepaymentRepository _loanRepaymentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEmailSender _emailService;

        public LoanService(IEmailVerificationRepository emailRepository, IEmailSender emailService,ITransactionRepository transactionRepository,ILoanRepaymentRepository loanRepaymentRepository,IUserRepository userRepository,IAccountRepository accountRepository, ILoanRepository loanRepository, IMapper mapper) {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _loanRepaymentRepository = loanRepaymentRepository;
            _transactionRepository = transactionRepository;
            _emailService = emailService;

        }

        public LoanDetails GetLoanDetails(InterestRequest request)
        {
            var totalAmount = PendingAmountCalculator(request.Amount, request.AppliedDate, request.TargetDate);
            var loanDetails = new LoanDetails
            {
                finalAmount = totalAmount,
                interestRate = ((totalAmount - request.Amount) / request.Amount) * 100,
            };
            return loanDetails;
        }
        public async Task<LoanRequest> ApplyLoan(Guid userId,LoanRequest request)
        {
            var user = await _userRepository.GetById(userId);  
          var account = await _accountRepository.GetById(request.AccountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            
            var loan = new Loan
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                Amount = request.Amount,
                Status = "Pending",
                AppliedDate = request.AppliedDate,
                TargetDate = request.TargetDate,
                PendingAmount = PendingAmountCalculator(request.Amount, request.AppliedDate, request.TargetDate)
            };
            await _loanRepository.Add(loan);
            await _emailService.SendEmailAsync(user.Email!, "Loan Application", $"Your loan application has been submitted successfully\n Loan Id : {loan.Id}");
            return _mapper.Map<LoanRequest>(loan);
        }

        private decimal PendingAmountCalculator(decimal amount ,DateTime start, DateTime end)
        {
            var difference = (end - start).TotalDays;
            decimal interestRate = 0.15m;
            if (difference < 30) 
            {
                interestRate = 0.02m;
            }
            else if (difference < 90)
            {
                interestRate = 0.05m;
            }
            else if (difference < 180)
            {
                interestRate = 0.1m;
            }
            return amount+(amount * interestRate);
        }

        public async Task<IEnumerable<LoanDto>> GetallAccountLoans(Guid userId,Guid accountId)
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
            var loans = await _loanRepository.GetLoansByAccountId(accountId);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        //User Add
        public async Task<LoanRepaymentDto> RepayLoanRequest(Guid userId, Guid loanId, LoanRepaymentDto request)
        {
            var loan = await _loanRepository.GetById(loanId);
            if (loan == null)
            {
                throw new LoanNotFoundException("Loan not found");
            }
            var account = await _accountRepository.GetById(loan.AccountId);
            if (account == null)
            {
                throw new AccountNotFoundException("Account not found");
            }
            if (account.UserId != userId)
            {
                throw new AccessViolationException("Access Denied");
            }
            if (loan.Status != "Opened")
            {
                throw new InvalidLoanStatusException("Loan is not approved!");
            }
            if (loan.PendingAmount < request.Amount)
            {
                throw new InvalidRepaymentAmountException("Invalid Repayment Amount");
            }
            if(loan.TargetDate < request.PaymentDate)
            {
                throw new DueDateException("Due Date Passed");
            }
           
            loan.PendingAmount -= request.Amount;
            
            if (loan.PendingAmount == 0)
            {
                loan.Status = "Closed";
                loan.RepaidDate = System.DateTime.UtcNow;
            }
            await _loanRepository.Update(loan);
            var loanRepayment = new LoanRepayment
            {
                Id = Guid.NewGuid(),
                LoanId = loanId,
                Amount = request.Amount,
                PaymentDate = System.DateTime.UtcNow
            };
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = loan.AccountId,
                ReceiverId = loan.AccountId,
                Amount = request.Amount,
                TransactionType = "Loan Payment",
                Timestamp = System.DateTime.UtcNow,
                Description = $"{loanId}",
                IsRecurring = false
            };
            account.Balance -= request.Amount;

            await _accountRepository.Update(account);
            await _transactionRepository.Add(transaction);
            await _loanRepaymentRepository.Add(loanRepayment);
            return _mapper.Map<LoanRepaymentDto>(loanRepayment);
        }

        //Admin
        public async Task<IEnumerable<LoanDto>> GetAllPendingLoansAsync()
        {
            var loans = await _loanRepository.GetAllPendingLoans();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
        public async Task ApproveLoanAsync(Guid loanId)
        {
            var loan = await _loanRepository.GetById(loanId);
            loan.Status = "Opened";
            await _loanRepository.Update(loan);
            var account = await _accountRepository.GetById(loan.AccountId);
            account.Balance += loan.Amount;
            await _accountRepository.Update(account);
            var user = await _userRepository.GetById(account.UserId);
            await _emailService.SendEmailAsync(user.Email!, "Your Loan Has been approved", $"ID : {loan.Id} \nNew Balance : {account.Balance}\nRepayment Amount : {loan.PendingAmount}\nDue Date: {loan.TargetDate.ToShortDateString}");
        }
        public async Task RejectLoanAsync(Guid loanId)
        {
            var loan = await _loanRepository.GetById(loanId);
            loan.Status = "Rejected";
            await _loanRepository.Update(loan);
        }
        public async Task<IEnumerable<LoanDto>> GetAllRejectedLoansAsync()
        {
            var loans = await _loanRepository.GetAllRejectedLoans();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
        public async Task <IEnumerable<LoanDto>> GetAllOpenedLoansAsync()
        {
            var loans = await _loanRepository.GetAllOpenedLoans();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
        public async Task <IEnumerable<LoanDto>> GetAllClosedLoansAsync()
        {
            var loans = await _loanRepository.GetAllClosedLoans();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
    }
}
