using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;

namespace SimpleBankingSystemAPI.Interfaces.Services
{
    public interface ILoanServices
    {
        LoanDetails GetLoanDetails(InterestRequest request);
        Task<LoanRequest> ApplyLoan(Guid userId,LoanRequest request);
        Task<IEnumerable<LoanDto>> GetallAccountLoans(Guid userId,Guid accountId);
        Task<IEnumerable<LoanDto>> GetAllPendingLoansAsync();
        Task ApproveLoanAsync(Guid loanId);
        Task RejectLoanAsync(Guid loanId);
        Task<IEnumerable<LoanDto>> GetAllOpenedLoansAsync();
        Task<IEnumerable<LoanDto>> GetAllClosedLoansAsync();
        Task<IEnumerable<LoanDto>> GetAllRejectedLoansAsync();
        Task<LoanRepaymentDto> RepayLoanRequest(Guid userId, Guid loanId, LoanRepaymentDto request);
        Task<IEnumerable<LoanRepaymentDto>> GetLoanRepayments(Guid loadId);
        Task<IEnumerable<LoanRepaymentDto>> GetAllRepaymentsForLoanID(Guid userId, Guid loadId);
    }
}
