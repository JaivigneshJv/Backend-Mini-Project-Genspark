using AutoMapper;
using Microsoft.Extensions.Logging;
using SimpleBankingSystemAPI.Exceptions;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using SimpleBankingSystemAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WatchDog;

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

        public LoanService(IEmailVerificationRepository emailRepository, IEmailSender emailService, ITransactionRepository transactionRepository, ILoanRepaymentRepository loanRepaymentRepository, IUserRepository userRepository, IAccountRepository accountRepository, ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _loanRepaymentRepository = loanRepaymentRepository;
            _transactionRepository = transactionRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Calculates the final amount and interest rate for a loan based on the provided request.
        /// </summary>
        /// <param name="request">The interest request containing the loan amount, applied date, and target date.</param>
        /// <returns>The calculated loan details.</returns>
        public LoanDetails GetLoanDetails(InterestRequest request)
        {
            try
            {
                WatchLogger.Log($"Calculating loan details for amount: {request.Amount}");
                var totalAmount = PendingAmountCalculator(request.Amount, request.AppliedDate, request.TargetDate);
                var loanDetails = new LoanDetails
                {
                    finalAmount = totalAmount,
                    interestRate = ((totalAmount - request.Amount) / request.Amount) * 100,
                };
                return loanDetails;
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error calculating loan details: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Applies for a loan for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user applying for the loan.</param>
        /// <param name="request">The loan request containing the account ID, amount, applied date, and target date.</param>
        /// <returns>The applied loan request.</returns>
        public async Task<LoanRequest> ApplyLoan(Guid userId, LoanRequest request)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to apply for a loan with account ID: {request.AccountId}");
                var user = await _userRepository.GetById(userId);
                var account = await _accountRepository.GetById(request.AccountId);

                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {request.AccountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {request.AccountId}");
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
                WatchLogger.Log($"Loan application submitted successfully. Loan ID: {loan.Id}");
                await _emailService.SendEmailAsync(user.Email!, "Loan Application", $"Your loan application has been submitted successfully\n Loan Id : {loan.Id}");
                return _mapper.Map<LoanRequest>(loan);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error applying for loan: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Calculates the pending amount for a loan.
        /// </summary>
        /// <param name="amount">The loan amount.</param>
        /// <param name="start">The start date of the loan.</param>
        /// <param name="end">The end date of the loan.</param>
        /// <returns>The calculated pending amount.</returns>
        private decimal PendingAmountCalculator(decimal amount, DateTime start, DateTime end)
        {
            try
            {
                WatchLogger.Log($"Calculating pending amount for loan from {start} to {end}");
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

                return amount + (amount * interestRate);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error calculating pending amount: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all loans for a specific account.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>A list of all loans for the account.</returns>
        public async Task<IEnumerable<LoanDto>> GetallAccountLoans(Guid userId, Guid accountId)
        {
            try
            {
                WatchLogger.Log($"Fetching all loans for account ID: {accountId} by user ID: {userId}");
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

                var loans = await _loanRepository.GetLoansByAccountId(accountId);
                return _mapper.Map<IEnumerable<LoanDto>>(loans);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching loans for account ID: {accountId}: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Processes a loan repayment request.
        /// </summary>
        /// <param name="userId">The ID of the user making the repayment.</param>
        /// <param name="loanId">The ID of the loan to be repaid.</param>
        /// <param name="request">The loan repayment request containing the repayment amount and payment date.</param>
        /// <returns>The processed loan repayment request.</returns>
        public async Task<LoanRepaymentDto> RepayLoanRequest(Guid userId, Guid loanId, LoanRepaymentDto request)
        {
            try
            {
                WatchLogger.Log($"User {userId} attempting to repay loan ID: {loanId}");
                var loan = await _loanRepository.GetById(loanId);
                if (loan == null)
                {
                    WatchLogger.LogWarning($"Loan not found for ID: {loanId}");
                    throw new LoanNotFoundException("Loan not found");
                }

                var account = await _accountRepository.GetById(loan.AccountId);
                if (account == null)
                {
                    WatchLogger.LogWarning($"Account not found for ID: {loan.AccountId}");
                    throw new AccountNotFoundException("Account not found");
                }
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {loan.AccountId}");
                    throw new AccessViolationException("Access Denied");
                }
                if (loan.Status != "Opened")
                {
                    WatchLogger.LogWarning($"Loan is not in an open state: {loanId}");
                    throw new InvalidLoanStatusException("Loan is not approved!");
                }
                if (loan.PendingAmount < request.Amount)
                {
                    WatchLogger.LogWarning($"Invalid repayment amount for loan ID: {loanId}");
                    throw new InvalidRepaymentAmountException("Invalid Repayment Amount");
                }
                if (loan.TargetDate < request.PaymentDate)
                {
                    WatchLogger.LogWarning($"Due date passed for loan ID: {loanId}");
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
                WatchLogger.Log($"Loan repayment processed successfully for loan ID: {loanId}");
                return _mapper.Map<LoanRepaymentDto>(loanRepayment);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error processing loan repayment for loan ID: {loanId}: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all pending loans.
        /// </summary>
        /// <returns>A list of all pending loans.</returns>
        public async Task<IEnumerable<LoanDto>> GetAllPendingLoansAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all pending loans");
                var loans = await _loanRepository.GetAllPendingLoans();
                return _mapper.Map<IEnumerable<LoanDto>>(loans);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all pending loans: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Approves a loan.
        /// </summary>
        /// <param name="loanId">The ID of the loan to be approved.</param>
        public async Task ApproveLoanAsync(Guid loanId)
        {
            try
            {
                WatchLogger.Log($"Approving loan ID: {loanId}");
                var loan = await _loanRepository.GetById(loanId);
                loan.Status = "Opened";
                await _loanRepository.Update(loan);
                var account = await _accountRepository.GetById(loan.AccountId);
                account.Balance += loan.Amount;
                await _accountRepository.Update(account);
                var user = await _userRepository.GetById(account.UserId);
                await _emailService.SendEmailAsync(user.Email!, "Your Loan Has been approved", $"ID : {loan.Id} \nNew Balance : {account.Balance}\nRepayment Amount : {loan.PendingAmount}\nDue Date: {loan.TargetDate.ToShortDateString}");
                WatchLogger.Log($"Loan approved successfully for loan ID: {loanId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error approving loan ID: {loanId}: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Rejects a loan.
        /// </summary>
        /// <param name="loanId">The ID of the loan to be rejected.</param>
        public async Task RejectLoanAsync(Guid loanId)
        {
            try
            {
                WatchLogger.Log($"Rejecting loan ID: {loanId}");
                var loan = await _loanRepository.GetById(loanId);
                loan.Status = "Rejected";
                await _loanRepository.Update(loan);
                WatchLogger.Log($"Loan rejected successfully for loan ID: {loanId}");
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error rejecting loan ID: {loanId}: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all rejected loans.
        /// </summary>
        /// <returns>A list of all rejected loans.</returns>
        public async Task<IEnumerable<LoanDto>> GetAllRejectedLoansAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all rejected loans");
                var loans = await _loanRepository.GetAllRejectedLoans();
                return _mapper.Map<IEnumerable<LoanDto>>(loans);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all rejected loans: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Gets all opened loans.
        /// </summary>
        /// <returns>A list of all opened loans.</returns>
        public async Task<IEnumerable<LoanDto>> GetAllOpenedLoansAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all opened loans");
                var loans = await _loanRepository.GetAllOpenedLoans();
                return _mapper.Map<IEnumerable<LoanDto>>(loans);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all opened loans: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all closed loans.
        /// </summary>
        /// <returns>A list of all closed loans.</returns>
        public async Task<IEnumerable<LoanDto>> GetAllClosedLoansAsync()
        {
            try
            {
                WatchLogger.Log("Fetching all closed loans");
                var loans = await _loanRepository.GetAllClosedLoans();
                return _mapper.Map<IEnumerable<LoanDto>>(loans);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all closed loans: {ex}");
                throw;
            }
        }
        /// <summary>
        /// Gets all repayments for a specific loan.
        /// </summary>
        /// <param name="loanId">The ID of the loan.</param>
        /// <returns>A list of all repayments for the loan.</returns>
        public async Task<IEnumerable<LoanRepaymentDto>> GetLoanRepayments(Guid loanId)
        {
            try
            {
                var loanrepayments = await _loanRepaymentRepository.GetLoanRepaymentsByLoanId(loanId);
                return _mapper.Map<IEnumerable<LoanRepaymentDto>>(loanrepayments);
            }catch(Exception ex)
            {
                WatchLogger.LogError($"Error fetching all loan repayments : {ex}");
                throw;
            }
        }
        public async Task<IEnumerable<LoanRepaymentDto>> GetAllRepaymentsForLoanID(Guid userId,Guid loadId)
        {
            try
            {
                var loan = await _loanRepository.GetById(loadId);
                var account = await _accountRepository.GetById(loan.AccountId);
                if (account.UserId != userId)
                {
                    WatchLogger.LogWarning($"Access denied for user ID: {userId} on account ID: {loan.AccountId}");
                    throw new AccessViolationException("Access Denied");
                }
                var loanrepayments = await _loanRepaymentRepository.GetLoanRepaymentsByLoanId(loadId);
                return _mapper.Map<IEnumerable<LoanRepaymentDto>>(loanrepayments);
            }
            catch (Exception ex)
            {
                WatchLogger.LogError($"Error fetching all loan repayments : {ex}");
                throw;
            }
        }
    }
}
