using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class TransactionVerificationRepository : Repository<Guid, TransactionVerification>, ITransactionVerificationRepository
    {
        private readonly BankingContext _context;

        public TransactionVerificationRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the transaction verification by account ID asynchronously.
        /// </summary>
        /// <param name="accountId">The ID of the account.</param>
        /// <returns>The transaction verification associated with the account ID.</returns>
        public async Task<TransactionVerification> GetVerificationByAccountIdAsync(Guid accountId)
        {
            return await _context.TransactionVerifications.SingleOrDefaultAsync(t => t.AccountId == accountId);
        }
    }
}
