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
        public async Task<IEnumerable<Loan>> GetLoansByAccountId(Guid accountId)
        {
            return await _context.Loans.Where(l => l.AccountId == accountId).ToListAsync();
        }
        public async Task<IEnumerable<Loan>> GetAllPendingLoans()
        {
            return await _context.Loans.Where(l=> l.Status == "Pending").ToListAsync();
        }
        public async Task<IEnumerable<Loan>> GetAllOpenedLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Opened").ToListAsync();
        }
        public async Task<IEnumerable<Loan>> GetAllClosedLoans()
        {
            return await _context.Loans.Where(l => l.Status == "Closed").ToListAsync();
        }
        public async Task<IEnumerable<Loan>> GetAllRejectedLoans()
        { 
            return await _context.Loans.Where(l => l.Status == "Rejected").ToListAsync();
        }

    }
}
