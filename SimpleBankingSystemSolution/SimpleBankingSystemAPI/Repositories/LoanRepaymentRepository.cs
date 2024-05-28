using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Retrieves the loan repayments by loan ID.
        /// </summary>
        /// <param name="loanId">The ID of the loan.</param>
        /// <returns>The list of loan repayments.</returns>
        public async Task<IEnumerable<LoanRepayment>> GetLoanRepaymentsByLoanId(Guid loanId)
        {
            return await _context.LoanRepayments.Where(x => x.LoanId == loanId).ToListAsync();
        }
    }
}
