using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class LoanRepository : Repository<Guid, Loan>, ILoanRepository
    {
        private readonly BankingContext _context;
        public LoanRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Get loans by account ID.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <returns>The loans associated with the account ID.</returns>
        public async Task<IEnumerable<Loan>> GetLoansByAccountId(Guid accountId)
        {
            return await _context.Loans.Where(l => l.AccountId == accountId).ToListAsync();
        }

        /// <summary>
        /// Get all pending loans.
        /// </summary>
        /// <returns>All pending loans.</returns>
        public async Task<IEnumerable<Loan>> GetAllPendingLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Pending").ToListAsync();
        }

        /// <summary>
        /// Get all opened loans.
        /// </summary>
        /// <returns>All opened loans.</returns>
        public async Task<IEnumerable<Loan>> GetAllOpenedLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Opened").ToListAsync();
        }

        /// <summary>
        /// Get all closed loans.
        /// </summary>
        /// <returns>All closed loans.</returns>
        public async Task<IEnumerable<Loan>> GetAllClosedLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Closed").ToListAsync();
        }

        /// <summary>
        /// Get all rejected loans.
        /// </summary>
        /// <returns>All rejected loans.</returns>
        public async Task<IEnumerable<Loan>> GetAllRejectedLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Rejected").ToListAsync();
        }
    }
}
