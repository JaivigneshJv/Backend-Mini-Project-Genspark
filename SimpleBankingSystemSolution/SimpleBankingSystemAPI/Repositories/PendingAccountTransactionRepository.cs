using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class PendingAccountTransactionRepository : Repository<Guid,PendingAccountTransaction>, IPendingAccountTransactionRepository
    {
        private readonly BankingContext _context;

        public PendingAccountTransactionRepository(BankingContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllTransactionByAccountId(Guid accountId)
        {
            return await _context.PendingAccountTransactions.Where(x => x.AccountId == accountId).ToListAsync();
        }
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllPendingTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsApproved == false && x.IsRejected == false).ToListAsync();
        }
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllAcceptedTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsApproved == true).ToListAsync();
        }
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllRejectedTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsRejected == true).ToListAsync();
        }
    }
}
