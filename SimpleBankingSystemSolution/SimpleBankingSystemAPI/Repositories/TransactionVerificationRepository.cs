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
        public async Task<TransactionVerification> GetVerificationByAccountIdAsync(Guid accountId)
        {
            return await _context.TransactionVerifications.SingleOrDefaultAsync(t => t.AccountId == accountId);
        }
    }
}
