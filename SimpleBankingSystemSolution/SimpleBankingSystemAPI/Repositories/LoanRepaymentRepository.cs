using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class LoanRepaymentRepository : Repository<Guid, LoanRepayment>, ILoanRepaymentRepository
    {
        private readonly BankingContext _context;
        public LoanRepaymentRepository(BankingContext context) : base(context)
        {
            _context = context;
        }       
    }
}
