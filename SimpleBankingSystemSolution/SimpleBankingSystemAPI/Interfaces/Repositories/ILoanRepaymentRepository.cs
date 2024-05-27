using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface ILoanRepaymentRepository : IRepository<Guid, LoanRepayment>
    {
    }
}
