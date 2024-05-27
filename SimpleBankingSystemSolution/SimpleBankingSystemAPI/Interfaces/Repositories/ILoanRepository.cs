using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface ILoanRepository : IRepository<Guid, Loan>
    {
        Task<IEnumerable<Loan>> GetLoansByAccountId(Guid accountId);
        Task<IEnumerable<Loan>> GetAllPendingLoans();
        Task<IEnumerable<Loan>> GetAllOpenedLoans();
        Task<IEnumerable<Loan>> GetAllClosedLoans();
        Task<IEnumerable<Loan>> GetAllRejectedLoans();

    }
}
