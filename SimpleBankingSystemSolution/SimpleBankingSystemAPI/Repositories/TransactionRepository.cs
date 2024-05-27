using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;

namespace SimpleBankingSystemAPI.Repositories
{
    public class TransactionRepository : Repository<Guid, Transaction>, ITransactionRepository
    {
        private readonly BankingContext _context;

        public TransactionRepository(BankingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
           return await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
        }
    }
}
