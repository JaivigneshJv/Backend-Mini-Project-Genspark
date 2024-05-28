using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class PendingAccountTransactionRepository : Repository<Guid, PendingAccountTransaction>, IPendingAccountTransactionRepository
    {
        private readonly BankingContext _context;

        public PendingAccountTransactionRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all pending account transactions for a specific account.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>A collection of pending account transactions.</returns>
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllTransactionByAccountId(Guid accountId)
        {
            return await _context.PendingAccountTransactions.Where(x => x.AccountId == accountId).ToListAsync();
        }

        /// <summary>
        /// Retrieves all pending account transactions that are neither approved nor rejected.
        /// </summary>
        /// <returns>A collection of pending account transactions.</returns>
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllPendingTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsApproved == false && x.IsRejected == false).ToListAsync();
        }

        /// <summary>
        /// Retrieves all accepted account transactions.
        /// </summary>
        /// <returns>A collection of accepted account transactions.</returns>
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllAcceptedTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsApproved == true).ToListAsync();
        }

        /// <summary>
        /// Retrieves all rejected account transactions.
        /// </summary>
        /// <returns>A collection of rejected account transactions.</returns>
        public async Task<IEnumerable<PendingAccountTransaction>> GetAllRejectedTransactions()
        {
            return await _context.PendingAccountTransactions.Where(x => x.IsRejected == true).ToListAsync();
        }
    }
}
